using Microsoft.EntityFrameworkCore;
using CustomerChargeNotification.Models;

namespace CustomerChargeNotification.DAL;

public class CustomerGameChargeContext : DbContext
{
    public CustomerGameChargeContext(DbContextOptions<CustomerGameChargeContext> options) : base(options) { }

    public DbSet<CustomerGameCharge> CustomerGameCharge { get; set; }
}
