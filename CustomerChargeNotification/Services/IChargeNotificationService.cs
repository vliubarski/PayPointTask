namespace CustomerChargeNotification.Services;

public interface IChargeNotificationService
{
    Task GenerateChargeNotifications(DateTime date);

}
