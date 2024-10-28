namespace CustomerChargeNotification.Services;

public class ChargeNotificationBackgroundService : BackgroundService
{
    private readonly IChargeNotificationService _notificationService;

    public ChargeNotificationBackgroundService(IChargeNotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _notificationService.GenerateChargeNotifications();
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
