using CustomerChargeNotification.DAL;
using CustomerChargeNotification.Models;

namespace CustomerChargeNotification.Domain;

public class ChargeNotificationProcessor : IChargeNotificationProcessor
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerGameChargeRepository _customerGameChargeRepository;

    public ChargeNotificationProcessor(
        ICustomerRepository customerRepo,
        ICustomerGameChargeRepository chargeRepo)
    {
        _customerRepository = customerRepo;
        _customerGameChargeRepository = chargeRepo;
    }


    public IEnumerable<ChargeNotification> GetChargeNotificationsForDate(DateTime date)
    {
        var charges = _customerGameChargeRepository.GetChargesForDate(date);
        var custIdsToCharge = charges.Select(x => x.CustomerId).Distinct();
        var customers = _customerRepository.GetCustomersByIds(custIdsToCharge);

        var res = charges
            .GroupBy(c => c.CustomerId)
            .Select(g => new ChargeNotification
            {
                CustomerId = g.Key,
                CustomerName = customers.First(x => x.Id == g.Key).Name,
                Charges = g.Select(x => new Charge { Date = date, Game = x.GameName, Cost = x.TotalCost }),
                Total = g.Sum(c => c.TotalCost)
            });

        return res;
    }
}
