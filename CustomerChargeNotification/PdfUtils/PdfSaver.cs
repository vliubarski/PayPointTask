using CustomerChargeNotification.PdfUtils;
using Microsoft.Extensions.Options;

namespace CustomerChargeNotification.PDFGeneration;

public class PdfSaver : IPdfSaver
{
    private readonly PdfSettings _settings;
    private readonly IFileSystem _fileSystem;

    public PdfSaver(IOptions<PdfSettings> settings, IFileSystem fileSystem)
    {
        _settings = settings.Value;
        _fileSystem = fileSystem;
    }

    public void SaveToFile(byte[] pdfData, string fileName)
    {
        if (!_fileSystem.DirectoryExists(_settings.OutputDirectory))
        {
            _fileSystem.CreateDirectory(_settings.OutputDirectory);
        }

        string filePath = Path.Combine(_settings.OutputDirectory, fileName);

        _fileSystem.WriteAllBytes(filePath, pdfData);
    }
}
