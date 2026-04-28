using CRFricke.EF.Core.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CA1515 // Consider making public types internal
#pragma warning disable CA1848 // Use the LoggerMessage delegates
#pragma warning disable CA1873 // Avoid potentially expensive logging

namespace EF.Core.Utilities.Test.Web.Data;

public class ApplicationDbContext : DbContext, ISeedingContext
{
    /// <summary>
    /// Used for integration testing with an in-memory database provider. 
    /// Do not use this constructor in production code.
    /// </summary>
    public ApplicationDbContext()
    { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>()
            .HasMany(e => e.Orders).WithOne().HasForeignKey(e => e.CustomerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>()
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

        if (await Customers.AnyAsync().ConfigureAwait(false))
        {
            return;
        }

        var customer = new Customer
        {
            Name = "Paints Are Us"
        }.SetOrders( new Order { }.SetItems("Paint", "Brushes", "GooBeGone") );

        var x = await Customers.AddAsync(customer).ConfigureAwait(false);
        logger.LogInformation($"{nameof(Customer)} '{customer.Name}' has been created.");

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            await SaveChangesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "SaveChangesAsync() method failed.");
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }
}
