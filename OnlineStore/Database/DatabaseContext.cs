using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineStore.Models;
using OnlineStore.Models.Database;
using OnlineStore.Models.Enums;

namespace OnlineStore.Database
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Item> Items { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Customer>()
                .Property(b => b.Code)
                .HasConversion(
                    v => v.ToString(),
                    v => CustomerCode.FromString(v));

            builder
                .Entity<Order>()
                .Property(e => e.Status)
                .HasConversion(new EnumToStringConverter<OrderStatus>());
        }
    }
}
