using CustomerChargeNotification.Models;

namespace CustomerChargeNotification.DAL;

public interface ICustomerRepository
{
    IEnumerable<Customer> GetCustomersByIds(IEnumerable<int> ids);
}
