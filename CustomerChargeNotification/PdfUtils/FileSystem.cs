
namespace CustomerChargeNotification.PdfUtils;

public class FileSystem : IFileSystem
{
    public bool DirectoryExists(string path) => Directory.Exists(path);

    public void CreateDirectory(string path) => Directory.CreateDirectory(path);

    public void WriteAllBytes(string path, byte[] bytes) => File.WriteAllBytes(path, bytes);
}