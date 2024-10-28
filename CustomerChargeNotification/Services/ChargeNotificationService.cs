using CustomerChargeNotification.Domain;
using CustomerChargeNotification.PDFGeneration;

namespace CustomerChargeNotification.Services;

public class ChargeNotificationService : IChargeNotificationService
{
    private readonly IChargeNotificationProcessor _chargeNotificationProcessor;
    private readonly IPdfGenerator _pdfGenerator;

    public ChargeNotificationService(
        IChargeNotificationProcessor processor,
        IPdfGenerator pdfGenerator)
    {
        _chargeNotificationProcessor = processor;
        _pdfGenerator = pdfGenerator;
    }

    public void GenerateChargeNotifications()
    {
        DateTime date = new DateTime(2021, 5, 20);
        var chargeNotifications = _chargeNotificationProcessor.GetChargeNotificationsForDate(date);

        foreach (var notification in chargeNotifications)
        {
            _pdfGenerator.Generate(notification);
        }
    }
}
