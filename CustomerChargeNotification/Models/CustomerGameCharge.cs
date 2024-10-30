﻿namespace CustomerChargeNotification.Models;

public class CustomerGameCharge
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public required string GameName { get; set; }
    public required string Description { get; set; }
    public int TotalCost { get; set; }
    public DateTime ChargeDate { get; set; }
}
