using CustomerChargeNotification.Models;

namespace CustomerChargeNotification.DAL;

public class CustomerGameChargeRepository : ICustomerGameChargeRepository
{
    private readonly CustomerGameChargeContext _context;
    private readonly ILogger<CustomerGameChargeRepository> _logger;

    public CustomerGameChargeRepository(CustomerGameChargeContext context,
        ILogger<CustomerGameChargeRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IEnumerable<CustomerGameCharge> GetChargesForDate(DateTime date)
    {
        _logger.LogInformation("Retrieving charges for date: {Date}", date);

        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

        var charges = _context.CustomerGameCharge
           .Where(charge => charge.ChargeDate >= startOfDay && charge.ChargeDate <= endOfDay)
           .ToList();

        _logger.LogInformation("Retrieved {Count} charges for date: {Date}", charges.Count, date);
        return charges;
    }
}
