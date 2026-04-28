using CRFricke.EF.Core.Utilities;
using EF.Core.Utilities.Test.Web.Areas.Identity;
using EF.Core.Utilities.Test.Web.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]

#pragma warning disable CA1515 // Consider making public types internal

namespace EF.Core.Utilities.Test.Web.Areas.Identity;

public class IdentityHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ConfigureServices((context, services) =>
        {
            services.AddDbContext<SecurityDbContext>(options =>
                options.UseSqlite(context.Configuration.GetConnectionString("IdentityContextConnection"))
                );

            services.AddDbInitializer(options =>
                options.UseDbContext<SecurityDbContext>(DbInitializationOption.Migrate)
            );

            services.AddDefaultIdentity<SecurityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<SecurityRole>()
                .AddEntityFrameworkStores<SecurityDbContext>();
        });
    }
}