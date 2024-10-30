namespace CustomerChargeNotification.BackgroundServices;

public interface IChargeNotificationService
{
    Task GenerateChargeNotifications(DateTime date);
}
