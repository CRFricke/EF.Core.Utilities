using CRFricke.EF.Core.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CA1812 // Class is apparently never instantiated and can be removed
#pragma warning disable CA1848 // Use the LoggerMessage delegates
#pragma warning disable CA1852 // Can be sealed because it has no subtypes in its containing assembly and is not externally visible
#pragma warning disable CA1873 // Avoid potentially expensive logging

namespace EF.Core.Utilities.Test.Web.Areas.Identity.Data;

internal class SecurityDbContext : IdentityDbContext<SecurityUser, SecurityRole, string>, ISeedingContext
{
    public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<SecurityRole>()
            .HasMany(e => e.Claims).WithOne().HasForeignKey(e => e.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<SecurityUser>()
            .HasMany(e => e.Claims).WithOne().HasForeignKey(e => e.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>")]
    public async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
    {
        var normalizer = serviceProvider.GetRequiredService<ILookupNormalizer>();
        var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<SecurityDbContext>();

        var role = await Roles.FindAsync(SecurityGuids.Role.Administrator).ConfigureAwait(false);
        if (role == null)
        {
            role = new SecurityRole
            {
                Id = SecurityGuids.Role.Administrator,
                Name = nameof(SecurityGuids.Role.Administrator),
                NormalizedName = normalizer.NormalizeName(nameof(SecurityGuids.Role.Administrator))
            };

            await Roles.AddAsync(role).ConfigureAwait(false);
            logger.LogInformation($"{nameof(SecurityRole)} '{role.Name}' has been created.");
        }

        var user = await Users.FindAsync(SecurityGuids.User.Administrator).ConfigureAwait(false);
        if (user == null)
        {
            var email = "Admin@company.com";

            user = new SecurityUser
            {
                Id = SecurityGuids.User.Administrator,
                Email = email,
                EmailConfirmed = true,
                LockoutEnabled = true,
                NormalizedEmail = normalizer.NormalizeEmail(email),
                NormalizedUserName = normalizer.NormalizeName(email),
                PasswordHash = "AQAAAAEAACcQAAAAEPPGh+zIZ8PSo5IQ1IjPnVqUph0c0utc5Kd37NmA8U1Fhe+MEu3gbxP81sPcxkJaMQ==", // "Administrat0r!"
                UserName = email
            }.SetClaims(role.Name!);

            await Users.AddAsync(user).ConfigureAwait(false);
            logger.LogInformation($"{nameof(SecurityUser)} '{user.Email}' has been created.");
        }

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
