using CustomerChargeNotification.DAL;
using CustomerChargeNotification.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CustomerChargeNotification.Domain;

public class ChargeNotificationProcessor : IChargeNotificationProcessor
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerGameChargeRepository _customerGameChargeRepository;
    private readonly ILogger<ChargeNotificationProcessor> _logger;

    public ChargeNotificationProcessor(
        ICustomerRepository customerRepo,
        ICustomerGameChargeRepository chargeRepo,
        ILogger<ChargeNotificationProcessor> logger)
    {
        _customerRepository = customerRepo ?? throw new ArgumentNullException(nameof(customerRepo));
        _customerGameChargeRepository = chargeRepo ?? throw new ArgumentNullException(nameof(chargeRepo));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IEnumerable<ChargeNotification> GetChargeNotificationsForDate(DateTime date)
    {
        _logger.LogInformation("Getting charge notifications for date: {Date}", date);
   
        var charges = _customerGameChargeRepository.GetChargesForDate(date).ToList();
        var custIdsToCharge = charges.Select(x => x.CustomerId).Distinct();
        var customers = _customerRepository.GetCustomersByIds(custIdsToCharge).ToList();

        // quick lookup of customer names by CustomerId
        var customerLookup = customers.ToDictionary(c => c.Id, c => c.Name);

        var notifications = charges
            .GroupBy(c => c.CustomerId)
            .Select(g => new ChargeNotification
            {
                CustomerId = g.Key,
                CustomerName = customerLookup.TryGetValue(g.Key, out var name) 
                    ? name 
                    : throw new ValueProviderException($"Customer with id {g.Key} has no name"),
                Charges = g.Select(x => new Charge { Date = date, Game = x.GameName, Cost = x.TotalCost }),
                Total = g.Sum(c => c.TotalCost)
            });

        _logger.LogInformation("Generated {Count} charge notifications for date: {Date}", notifications.Count(), date);
        return notifications;
    }
}
