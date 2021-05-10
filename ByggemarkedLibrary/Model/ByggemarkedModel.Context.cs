
using System.Data.Entity;


namespace ByggemarkedLibrary.Model
{
    public class ByggemarkedContext : DbContext
    {
        public ByggemarkedContext() : base("name=Byggemarked") { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Tool> Tools { get; set; }
    }
}