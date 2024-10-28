using CustomerChargeNotification.Models;

namespace CustomerChargeNotification.DAL;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerContext _context;
    private readonly ILogger<CustomerRepository> _logger;

    public CustomerRepository(CustomerContext context, ILogger<CustomerRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IEnumerable<Customer> GetCustomersByIds(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
        {
            _logger.LogWarning("No customer IDs provided to retrieve.");
            return Enumerable.Empty<Customer>();
        }

        var customers = _context.Customer
            .Where(customer => ids.Contains(customer.Id))
            .ToList();
        _logger.LogInformation("Retrieved {Count} customers for IDs: {Ids}", customers.Count, string.Join(", ", ids));

        return customers;
    }
}
