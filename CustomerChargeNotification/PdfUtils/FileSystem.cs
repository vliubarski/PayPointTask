
namespace CustomerChargeNotification.PdfUtils;

public class FileSystem : IFileSystem
{
    public bool DirectoryExists(string path) => Directory.Exists(path);

    public void CreateDirectory(string path) => Directory.CreateDirectory(path);

    public Task WriteAllBytesAsync(string path, byte[] bytes) => File.WriteAllBytesAsync(path, bytes);
}