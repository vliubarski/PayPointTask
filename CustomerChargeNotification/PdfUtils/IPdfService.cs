using CustomerChargeNotification.Models;

namespace CustomerChargeNotification.PDFGeneration
{
    public interface IPdfService
    {
        void SaveToFile(ChargeNotification chargeNotification);
    }
}