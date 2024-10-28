using CustomerChargeNotification.Models;

namespace CustomerChargeNotification.DAL;

public interface ICustomerGameChargeRepository
{
    IEnumerable<CustomerGameCharge> GetChargesForDate(DateTime date);
}
