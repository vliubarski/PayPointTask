using PayPoint.Models;
using System.Data.SqlClient;

namespace PayPoint.DAL;

public class CustomerRepository : ICustomerRepository
{
    private readonly SqlConnection _connection;

    public CustomerRepository(SqlConnection connection)
    {
        _connection = connection;
    }

    public Customer GetCustomerById(int customerId)
    {
        // todo: Query database to get customer details
        return new Customer { Id = 1, Name = "test" };
    }

    public List<Customer> GetCustomersBatch(int batchSize, int skip)
    {
        // todo: Query database to get customers in batches
        return new List<Customer> { new Customer { Id = 1, Name = "test" } };
    }
}
