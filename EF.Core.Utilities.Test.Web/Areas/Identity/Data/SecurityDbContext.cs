using CRFricke.EF.Core.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EF.Core.Utilities.Test.Web.Areas.Identity.Data
{
    public class SecurityDbContext : IdentityDbContext<SecurityUser, SecurityRole, string>, ISeedingContext
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

        public async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
        {
            var normalizer = serviceProvider.GetRequiredService<ILookupNormalizer>();
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<SecurityDbContext>();

            var role = await Roles.FindAsync(SecurityGuids.Role.Administrator);
            if (role == null)
            {
                role = new SecurityRole
                {
                    Id = SecurityGuids.Role.Administrator,
                    Name = nameof(SecurityGuids.Role.Administrator),
                    NormalizedName = normalizer.NormalizeName(nameof(SecurityGuids.Role.Administrator))
                };

                await Roles.AddAsync(role);
                logger.LogInformation($"{nameof(SecurityRole)} '{role.Name}' has been created.");
            }

            var user = await Users.FindAsync(SecurityGuids.User.Administrator);
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
                }.SetClaims(role.Name);

                await Users.AddAsync(user);
                logger.LogInformation($"{nameof(SecurityUser)} '{user.Email}' has been created.");
            }

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
