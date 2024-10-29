using CustomerChargeNotification.PDFGeneration;
using Moq;
using CustomerChargeNotification.PdfUtils;
using Microsoft.Extensions.Options;

namespace CustomerChargeNotificationTests.PdfUtils;

[TestFixture]
public class PdfSaverTests
{
    private Mock<IOptions<PdfSettings>> _mockOptions;
    private PdfSaver _pdfSaver;
    private Mock<IFileSystem> _mockFileSystem; 
    private PdfSettings _pdfSettings;

    [SetUp]
    public void SetUp()
    {
        _mockFileSystem = new Mock<IFileSystem>();
        var testDirectory = Path.Combine(Path.GetTempPath(), "TestPdfOutput");

        _pdfSettings = new PdfSettings
        {
            OutputDirectory = testDirectory
        };

        _mockOptions = new Mock<IOptions<PdfSettings>>();
        _mockOptions.Setup(o => o.Value).Returns(_pdfSettings);

        _pdfSaver = new PdfSaver(_mockOptions.Object, _mockFileSystem.Object);
    }

    [Test]
    public void SaveToFile_ShouldCreateDirectory_IfNotExists()
    {
        // Arrange
        byte[] pdfData = [1, 2, 3];
        string fileName = "test.pdf";

        _mockFileSystem.Setup(fs => fs.DirectoryExists(_pdfSettings.OutputDirectory)).Returns(false);

        // Act
        _pdfSaver.SaveToFile(pdfData, fileName);

        // Assert
        _mockFileSystem.Verify(fs => fs.CreateDirectory(_pdfSettings.OutputDirectory), Times.Once);
    }

    [Test]
    public void SaveToFile_ShouldSavePdfFile_AtCorrectPath()
    {
        // Arrange
        byte[] pdfData = [1, 2, 3];
        string fileName = "test.pdf";
        string expectedFilePath = Path.Combine(_pdfSettings.OutputDirectory, fileName);

        _mockFileSystem.Setup(fs => fs.DirectoryExists(_pdfSettings.OutputDirectory)).Returns(true);

        // Act
        _pdfSaver.SaveToFile(pdfData, fileName);

        // Assert
        _mockFileSystem.Verify(fs => fs.WriteAllBytes(expectedFilePath, pdfData), Times.Once);
    }

    [Test]
    public void SaveToFile_ShouldNotCreateDirectory_IfAlreadyExists()
    {
        // Arrange
        byte[] pdfData = [1, 2, 3];
        string fileName = "test.pdf";

        _mockFileSystem.Setup(fs => fs.DirectoryExists(_pdfSettings.OutputDirectory)).Returns(true);

        // Act
        _pdfSaver.SaveToFile(pdfData, fileName);

        // Assert
        _mockFileSystem.Verify(fs => fs.CreateDirectory(It.IsAny<string>()), Times.Never, "CreateDirectory should not be called if directory already exists.");
    }

    [Test]
    public void SaveToFile_ShouldThrow_Exception_WhenFileWriteFails()
    {
        // Arrange
        byte[] pdfData = [1, 2, 3];
        string fileName = "test.pdf";
        string expectedFilePath = Path.Combine(_pdfSettings.OutputDirectory, fileName);

        _mockFileSystem.Setup(fs => fs.DirectoryExists(_pdfSettings.OutputDirectory)).Returns(true);

        _mockFileSystem.Setup(fs => fs.WriteAllBytes(expectedFilePath, pdfData)).Throws(new UnauthorizedAccessException("Access denied."));

        // Act & Assert
        var ex = Assert.Throws<UnauthorizedAccessException>(() => _pdfSaver.SaveToFile(pdfData, fileName));
        Assert.That(ex.Message, Is.EqualTo("Access denied."), "Should throw UnauthorizedAccessException when write access is denied.");
    }
}