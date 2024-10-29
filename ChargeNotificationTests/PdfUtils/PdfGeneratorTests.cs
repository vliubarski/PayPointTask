using CustomerChargeNotification.Models;
using CustomerChargeNotification.PDFGeneration;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using Moq;

namespace CustomerChargeNotificationTests.PdfUtils;

[TestFixture]
public class PdfGeneratorTests
{
    private Mock<Func<MemoryStream>> _mockMemoryStreamFactory;
    private Mock<Func<MemoryStream, PdfWriter>> _mockPdfWriterFactory;
    private PdfGenerator _pdfGenerator;
    private MemoryStream _testMemoryStream;

    [SetUp]
    public void SetUp()
    {
        _mockMemoryStreamFactory = new Mock<Func<MemoryStream>>();
        _mockPdfWriterFactory = new Mock<Func<MemoryStream, PdfWriter>>();
        _testMemoryStream = new MemoryStream();

        _mockMemoryStreamFactory.Setup(m => m()).Returns(_testMemoryStream);
        _mockPdfWriterFactory.Setup(m => m(It.IsAny<MemoryStream>())).Returns((MemoryStream ms) => new PdfWriter(ms));

        _pdfGenerator = new PdfGenerator(_mockMemoryStreamFactory.Object, _mockPdfWriterFactory.Object);
    }

    [Test]
    public void GetPdfData_GeneratesPdfWithCustomerDetails()
    {
        // Arrange
        var notification = new ChargeNotification
        {
            CustomerId = 12345,
            CustomerName = "John Doe",
            Total = 200,
            Charges = new List<Charge>()
        };

        // Act
        var pdfData = _pdfGenerator.GetPdfData(notification);

        // Assert
        Assert.That(pdfData, Is.Not.Empty);

        using var pdfDoc = new PdfDocument(new PdfReader(new MemoryStream(pdfData)));
        var pdfContent = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(1));
        Assert.That(pdfContent, Does.Contain("Customer Number: 12345"));
        Assert.That(pdfContent, Does.Contain("Customer Name: John Doe"));
    }

    [Test]
    public void GetPdfData_GeneratesPdfWithChargesTable()
    {
        // Arrange
        var notification = new ChargeNotification
        {
            CustomerId = 12345,
            CustomerName = "John Doe",
            Total = 200,
            Charges = new List<Charge>
                {
                    new Charge { Date = new DateTime(2023, 1, 1), Game = "Game A", Cost = 50 },
                    new Charge { Date = new DateTime(2023, 1, 2), Game = "Game B", Cost = 150 }
                }
        };

        // Act
        var pdfData = _pdfGenerator.GetPdfData(notification);

        // Assert
        Assert.That(pdfData, Is.Not.Empty);

        using var pdfDoc = new PdfDocument(new PdfReader(new MemoryStream(pdfData)));
        var pdfContent = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(1));
        Assert.That(pdfContent, Does.Contain("CHARGES"));
        Assert.That(pdfContent, Does.Contain("01/01/2023"));
        Assert.That(pdfContent, Does.Contain("Game A"));
        Assert.That(pdfContent, Does.Contain("50"));
        Assert.That(pdfContent, Does.Contain("02/01/2023"));
        Assert.That(pdfContent, Does.Contain("Game B"));
        Assert.That(pdfContent, Does.Contain("150"));
    }

    [Test]
    public void GetPdfData_GeneratesPdfWithTotalAmount()
    {
        // Arrange
        var notification = new ChargeNotification
        {
            CustomerId = 12345,
            CustomerName = "John Doe",
            Total = 200,
            Charges = new List<Charge>
                {
                    new Charge { Date = new DateTime(2023, 1, 1), Game = "Game A", Cost = 50 },
                    new Charge { Date = new DateTime(2023, 1, 2), Game = "Game B", Cost = 150 }
                }
        };

        // Act
        var pdfData = _pdfGenerator.GetPdfData(notification);

        // Assert
        Assert.That(pdfData, Is.Not.Empty);

        using var pdfDoc = new PdfDocument(new PdfReader(new MemoryStream(pdfData)));
        var pdfContent = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(1));
        Assert.That(pdfContent, Does.Contain("TOTAL (pence): 200"));
    }

    [Test]
    public void GetPdfData_ThrowsArgumentNullException_WhenFactoriesAreNull()
    {
        // Arrange, Act, Assert
        Assert.Throws<ArgumentNullException>(() => new PdfGenerator(null, null));
    }
}