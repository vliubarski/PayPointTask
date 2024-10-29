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

    public async Task SaveToFileAsync(byte[] pdfData, string fileName)
    {
        if (!_fileSystem.DirectoryExists(_settings.OutputDirectory))
        {
            _fileSystem.CreateDirectory(_settings.OutputDirectory);
        }

        string filePath = Path.Combine(_settings.OutputDirectory, fileName);

        await _fileSystem.WriteAllBytesAsync(filePath, pdfData);
    }
}
