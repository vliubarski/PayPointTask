using Microsoft.EntityFrameworkCore;
using CustomerChargeNotification.Models;

namespace CustomerChargeNotification.DAL;

public class CustomerContext : DbContext
{
    public CustomerContext(DbContextOptions<CustomerContext> options) : base(options) { }

    public DbSet<Customer> Customer { get; set; }
}
