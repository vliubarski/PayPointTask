using CustomerChargeNotification.Models;

namespace CustomerChargeNotification.PDFGeneration;

public interface IPdfGenerator
{
    void Generate(ChargeNotification notification);
}