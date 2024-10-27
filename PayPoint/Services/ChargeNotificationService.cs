using PayPoint.DAL;
using PayPoint.Domain;
using PayPoint.PDFGeneration;

namespace PayPoint.Services;

public class ChargeNotificationService : IChargeNotificationService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IChargeNotificationProcessor _chargeNotificationProcessor;
    private readonly IPdfGenerator _pdfGenerator;

    public ChargeNotificationService(ICustomerRepository customerRepository,
        IChargeNotificationProcessor processor,
        IPdfGenerator pdfGenerator)
    {
        _customerRepository = customerRepository;
        _chargeNotificationProcessor = processor;
        _pdfGenerator = pdfGenerator;
    }

    public void GenerateChargeNotifications()
    {
        // todo delete this magic nums
        var batchSize = 1000;
        var totalCustomers = 30000; 

        for (int i = 0; i < totalCustomers; i += batchSize)
        {
            var customers = _customerRepository.GetCustomersBatch(batchSize, i);

            foreach (var customer in customers)
            {
                var notification = _chargeNotificationProcessor.ProcessCustomerCharges(customer.Id);
                _pdfGenerator.Generate(notification);
            }
        }
    }
}
