namespace CustomerChargeNotification.Services;

public interface IChargeNotificationService
{
    void GenerateChargeNotifications(DateTime date);
}
