using CustomerChargeNotification.PDFGeneration;
using Moq;
using CustomerChargeNotification.Models;

namespace CustomerChargeNotificationTests.PdfUtils;

[TestFixture]
public class PdfServiceTests
{
    private Mock<IPdfGenerator> _mockPdfGenerator;
    private Mock<IPdfSaver> _mockPdfSaver;
    private PdfService _pdfService;

    [SetUp]
    public void SetUp()
    {
        _mockPdfGenerator = new Mock<IPdfGenerator>();
        _mockPdfSaver = new Mock<IPdfSaver>();
        _pdfService = new PdfService(_mockPdfGenerator.Object, _mockPdfSaver.Object);
    }

    [Test]
    public async Task SaveToFile_ShouldGeneratePdfData_AndSaveToFile()
    {
        // Arrange
        var chargeNotification = new ChargeNotification
        {
            CustomerName = "John Doe",
            Charges = new List<Charge>()
        };

        byte[] pdfData = [1, 2, 3];
        string expectedFileName = $"{DateTime.UtcNow:yyyy-MM-dd}{chargeNotification.CustomerName}.pdf";

        // Set up the mock to return some PDF data
        _mockPdfGenerator.Setup(g => g.GetPdfData(chargeNotification)).Returns(pdfData);

        // Act
        await _pdfService.SaveToFileAsync(chargeNotification);

        // Assert
        _mockPdfGenerator.Verify(g => g.GetPdfData(chargeNotification), Times.Once, "GetPdfData should be called once.");
        _mockPdfSaver.Verify(s => s.SaveToFileAsync(pdfData, It.IsAny<string>()), Times.Once, "SaveToFile should be called once.");

        // Verify the correct filename is being used
        // Since DateTime.UtcNow is used in the filename, we check that the file name starts with the date format.
        string actualFileName = $"{DateTime.UtcNow:yyyy-MM-dd} {chargeNotification.CustomerName}.pdf";
        _mockPdfSaver.Verify(s => s.SaveToFileAsync(pdfData, actualFileName), Times.Once, "SaveToFile should be called with the correct filename.");
    }
}