namespace CustomerChargeNotification.Models;

public class ChargeNotification
{
    public int CustomerId { get; set; }
    public required string CustomerName { get; set; }
    public required IEnumerable<Charge> Charges { get; set; }
    public int Total { get; set; }
}

public record struct Charge(DateTime Date, string Game, int Cost);