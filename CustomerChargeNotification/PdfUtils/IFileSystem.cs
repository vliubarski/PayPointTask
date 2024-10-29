namespace CustomerChargeNotification.PdfUtils;

public interface IFileSystem
{
    void CreateDirectory(string path);
    bool DirectoryExists(string path);
    Task WriteAllBytesAsync(string path, byte[] bytes) => File.WriteAllBytesAsync(path, bytes);
}