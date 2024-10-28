using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CustomerChargeNotification.Services;

public interface IChargeNotificationService
{
    void GenerateChargeNotifications(DateTime date);
}
