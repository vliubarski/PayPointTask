using PayPoint.Models;

namespace PayPoint.DAL;

public interface ICustomerGameChargeRepository
{
    List<CustomerGameCharge> GetChargesForCustomer(int customerId);
}
