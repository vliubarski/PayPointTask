namespace PayPoint.Models;

public class ChargeNotification
{
    public int CustomerId { get; set; }
    public required string CustomerName { get; set; }
    public double Charges { get; set; }
}
