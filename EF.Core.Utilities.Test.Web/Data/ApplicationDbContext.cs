using CRFricke.EF.Core.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EF.Core.Utilities.Test.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, ISeedingContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationRole>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }


        /// <inheritdoc/>
        public async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
        {
            var normalizer = serviceProvider.GetRequiredService<ILookupNormalizer>();
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<ApplicationDbContext>();

            var role = await Roles.FindAsync(AppGuids.Role.CalendarManager);
            if (role == null)
            {
                role = new ApplicationRole
                {
                    Id = AppGuids.Role.CalendarManager,
                    Name = nameof(AppGuids.Role.CalendarManager),
                    Description = "CalendarManagers are responsible for managing the company's calendar.",
                    NormalizedName = normalizer.NormalizeName(nameof(AppGuids.Role.CalendarManager))
                }.SetClaims(AppClaims.Calendar.DefinedClaims);

                await Roles.AddAsync(role);
                logger.LogInformation($"{nameof(ApplicationRole)} '{role.Name}' has been created.");
            }

            var user = await Users.FindAsync(AppGuids.User.CalendarGuy);
            if (user == null)
            {
                var email = "CalendarGuy@company.com";

                user = new ApplicationUser
                {
                    Id = AppGuids.User.CalendarGuy,
                    Email = email,
                    EmailConfirmed = true,
                    GivenName = "Julian",
                    LockoutEnabled = false,
                    NormalizedEmail = normalizer.NormalizeEmail(email),
                    NormalizedUserName = normalizer.NormalizeName(email),
                    PasswordHash = "AQAAAAEAACcQAAAAEFXwSRmwaiwTjRDW4zcaupENlSdbverXypglebb + Ti6f / Rn4sBikU3q / uE0jJQJAMw ==", // "Calend@rGuy!"
                    Surname = "Day",
                    UserName = email
                }.SetClaims(new[] { role.Name });

                await Users.AddAsync(user);
                logger.LogInformation($"{nameof(ApplicationUser)} '{user.Email}' has been created.");
            }

            role = await Roles.FindAsync(AppGuids.Role.DocumentManager);
            if (role == null)
            {
                role = new ApplicationRole
                {
                    Id = AppGuids.Role.DocumentManager,
                    Name = nameof(AppGuids.Role.DocumentManager),
                    Description = "DocumentManagers are responsible for managing the company's documents.",
                    NormalizedName = normalizer.NormalizeName(nameof(AppGuids.Role.DocumentManager))
                }.SetClaims(AppClaims.Document.DefinedClaims);

                await Roles.AddAsync(role);
                logger.LogInformation($"{nameof(ApplicationRole)} '{role.Name}' has been created.");
            }

            user = await Users.FindAsync(AppGuids.User.DocumentGuy);
            if (user == null)
            {
                var email = "DocumentGuy@company.com";

                user = new ApplicationUser
                {
                    Id = AppGuids.User.DocumentGuy,
                    Email = email,
                    EmailConfirmed = true,
                    GivenName = "Mark",
                    LockoutEnabled = false,
                    NormalizedEmail = normalizer.NormalizeEmail(email),
                    NormalizedUserName = normalizer.NormalizeName(email),
                    PasswordHash = "AQAAAAEAACcQAAAAEJaNzNSqF3SrSxUHuT010YO6kAmf95+Xv20mzd3MzLBTNU8ySBGMBqkx82q3Be+BCg==", // "D0cumentGuy!"
                    Surname = "Hofmann",
                    UserName = email
                }.SetClaims(new[] { role.Name });

                await Users.AddAsync(user);
                logger.LogInformation($"{nameof(ApplicationUser)} '{user.Email}' has been created.");
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
