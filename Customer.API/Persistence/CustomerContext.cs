using Customer.API.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Persistence
{
    public class CustomerContext: DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Address { get; set; }
    }
}