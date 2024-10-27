using PayPoint.Models;
using System.Data.SqlClient;

namespace PayPoint.DAL;

public class CustomerGameChargeRepository : ICustomerGameChargeRepository
{
    private readonly SqlConnection _connection;

    public CustomerGameChargeRepository(SqlConnection connection)
    {
        _connection = connection;
    }

    public List<CustomerGameCharge> GetChargesForCustomer(int customerId)
    {
        // todo: Query database to get customer charges
        return new List<CustomerGameCharge> { };
    }
}
