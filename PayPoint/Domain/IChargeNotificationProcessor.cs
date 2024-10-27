using PayPoint.Models;

namespace PayPoint.Domain;

public interface IChargeNotificationProcessor
{
    ChargeNotification ProcessCustomerCharges(int customerId);
}
