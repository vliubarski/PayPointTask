using PayPoint.DAL;
using PayPoint.Models;

namespace PayPoint.Domain;

public class ChargeNotificationProcessor : IChargeNotificationProcessor
{
    private readonly ICustomerRepository _customerRepo;
    private readonly ICustomerGameChargeRepository _customerGameChargeRepository;

    public ChargeNotificationProcessor(
        ICustomerRepository customerRepo,
        ICustomerGameChargeRepository chargeRepo)
    {
        _customerRepo = customerRepo;
        _customerGameChargeRepository = chargeRepo;
    }

    public ChargeNotification ProcessCustomerCharges(int customerId)
    {
        var customer = _customerRepo.GetCustomerById(customerId);
        var charges = _customerGameChargeRepository.GetChargesForCustomer(customerId);

        // Aggregate charges by game and calculate totals
        var groupedCharges = charges
            .GroupBy(c => c.GameName)
            .Select(g => new {
                Game = g.Key,
                Total = g.Sum(c => c.TotalCost)
            }).ToList();

        // Create a charge notification
        var notification = new ChargeNotification
        {
            CustomerId = customer.Id,
            CustomerName = customer.Name,
            Charges = groupedCharges.Sum(x=>x.Total) // todo : check it!
        };

        return notification;
    }
}
