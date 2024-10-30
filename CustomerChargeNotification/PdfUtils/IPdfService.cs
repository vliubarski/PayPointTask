using CustomerChargeNotification.Models;

namespace CustomerChargeNotification.PDFGeneration;

public interface IPdfService
{
    Task SaveToFileAsync(ChargeNotification chargeNotification);
}