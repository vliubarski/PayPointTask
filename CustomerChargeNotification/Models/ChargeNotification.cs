namespace CustomerChargeNotification.Models;

public class ChargeNotification
{
    public int CustomerId { get; set; }
    public required string CustomerName { get; set; }
    public required IEnumerable<Charge> Charges { get; set; }
    public double Total { get; set; }
}

public record struct Charge(DateTime Date, string Game, double Cost);