namespace CustomerChargeNotification.PDFGeneration
{
    public interface IPdfSaver
    {
        void SaveToFile(byte[] pdfData, string fileName);
    }
}