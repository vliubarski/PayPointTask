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

    public async Task GenerateChargeNotifications(DateTime date)
    {
        _logger.LogInformation("Charge Notifications generating started at {time}", DateTime.UtcNow);

        var chargeNotifications = _chargeNotificationProcessor.GetChargeNotificationsForDate(date);

        const int batchSize = 100;
        const int maxParallelism = 20;

        // Process notifications in batches
        var batchTasks = new List<Task>();
        foreach (var batch in chargeNotifications.Batch(batchSize))
        {
            var batchTask = Task.Run(async () =>
            {
                await Parallel.ForEachAsync(batch, new ParallelOptions { MaxDegreeOfParallelism = maxParallelism }, async (notification, _) =>
                {
                    try
                    {
                        await _pdfService.SaveToFileAsync(notification);
                        _logger.LogInformation("Successfully generated PDF for CustomerId {CustomerId}.", notification.CustomerId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to generate PDF for CustomerId {CustomerId}", notification.CustomerId);
                        throw;
                    }
                });
            });
            batchTasks.Add(batchTask);
        }

        await Task.WhenAll(batchTasks);
        _logger.LogInformation("Charge Notifications generating finished at {time}", DateTime.UtcNow);
    }
}
