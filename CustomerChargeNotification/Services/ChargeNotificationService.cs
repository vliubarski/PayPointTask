using CustomerChargeNotification.Domain;
using CustomerChargeNotification.PDFGeneration;

namespace CustomerChargeNotification.Services;

public class ChargeNotificationService : IChargeNotificationService
{
    private readonly IChargeNotificationProcessor _chargeNotificationProcessor;
    private readonly IPdfService _pdfService;
    private readonly ILogger<ChargeNotificationService> _logger;

    public ChargeNotificationService(IChargeNotificationProcessor processor,
        IPdfService pdfService, ILogger<ChargeNotificationService> logger)
    {
        _chargeNotificationProcessor = processor ?? throw new ArgumentNullException(nameof(processor));
        _pdfService = pdfService ?? throw new ArgumentNullException(nameof(pdfService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void GenerateChargeNotifications(DateTime date)
    {
        var chargeNotifications = _chargeNotificationProcessor.GetChargeNotificationsForDate(date);

        foreach (var notification in chargeNotifications)
        {
            try
            {
                _pdfService.SaveToFile(notification);
                _logger.LogInformation("Successfully generated PDF for CustomerId {CustomerId}.", notification.CustomerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate PDF for CustomerId {CustomerId}", notification.CustomerId);
                throw;
            }
        }
    }
}
