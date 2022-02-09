using CRFricke.EF.Core.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EF.Core.Utilities.Test.Web.Data
{
    public class ApplicationDbContext : DbContext, ISeedingContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Customer>()
                .HasMany(e => e.Orders).WithOne().HasForeignKey(e => e.CustomerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>()
                .HasMany(e => e.Items).WithOne().HasForeignKey(e => e.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>")]
        public async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
        {
            var normalizer = serviceProvider.GetRequiredService<ILookupNormalizer>();
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<ApplicationDbContext>();

            if (await Customers.AnyAsync())
            {
                return;
            }

            var customer = new Customer
            {
                Name = "Paints Are Us"
            }.SetOrders( new Order { }.SetItems("Paint", "Brushes", "GooBeGone") );

            var x = await Customers.AddAsync(customer);
            logger.LogInformation($"{nameof(Customer)} '{customer.Name}' has been created.");

            try
            {
                await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "SaveChangesAsync() method failed.");
            }
        }
    }
}
