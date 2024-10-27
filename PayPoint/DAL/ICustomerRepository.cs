using PayPoint.Models;

namespace PayPoint.DAL;

public interface ICustomerRepository
{
    Customer GetCustomerById(int customerId);
    List<Customer> GetCustomersBatch(int batchSize, int skip);
}
