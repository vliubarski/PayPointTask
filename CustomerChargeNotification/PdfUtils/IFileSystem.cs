namespace CustomerChargeNotification.PdfUtils;

public interface IFileSystem
{
    void CreateDirectory(string path);
    bool DirectoryExists(string path);
    void WriteAllBytes(string path, byte[] bytes);
}