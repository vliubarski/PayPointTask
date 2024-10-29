using CustomerChargeNotification.Models;

namespace CustomerChargeNotification.PDFGeneration;

public interface IPdfGenerator
{
    byte[] GetPdfData(ChargeNotification notification);
}