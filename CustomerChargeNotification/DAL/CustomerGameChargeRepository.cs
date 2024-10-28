using CustomerChargeNotification.Models;

namespace CustomerChargeNotification.DAL;

public class CustomerGameChargeRepository : ICustomerGameChargeRepository
{
    private readonly CustomerGameChargeContext _context;

    public CustomerGameChargeRepository(CustomerGameChargeContext context)
    {
        _context = context;
    }

    public IEnumerable<CustomerGameCharge> GetChargesForDate(DateTime date)
    {
        return _context.CustomerGameCharge
           .Where(charge => charge.ChargeDate == date)
           .ToList();
    }
}
