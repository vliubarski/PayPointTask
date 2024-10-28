using CustomerChargeNotification.Models;

namespace CustomerChargeNotification.DAL;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerContext _context;

    public CustomerRepository(CustomerContext context)
    {
        _context = context;
    }

    public IEnumerable<Customer> GetCustomersByIds(IEnumerable<int> ids)
    {
        var res = _context.Customer
        .Join(ids,
              customer => customer.Id,
              id => id,
              (customer, id) => customer
             );

        return res;
    }
}
