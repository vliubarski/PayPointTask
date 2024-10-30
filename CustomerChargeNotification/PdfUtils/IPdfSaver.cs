namespace CustomerChargeNotification.PDFGeneration;

public interface IPdfSaver
{
    Task SaveToFileAsync(byte[] pdfData, string fileName);
}