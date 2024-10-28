using CustomerChargeNotification.Models;

namespace CustomerChargeNotification.Domain;

public interface IChargeNotificationProcessor
{
    IEnumerable<ChargeNotification> GetChargeNotificationsForDate(DateTime date);
}
