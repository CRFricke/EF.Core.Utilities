﻿using CRFricke.EF.Core.Utilities;
using EF.Core.Utilities.Test.Web.Areas.Identity;
using EF.Core.Utilities.Test.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]
namespace EF.Core.Utilities.Test.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<SecurityDbContext>(options =>
                    options.UseSqlite(context.Configuration.GetConnectionString("IdentityContextConnection"))
                    );

                services.AddDbInitializer(options =>
                    options.UseDbContext(typeof(SecurityDbContext), DbInitializationOption.Migrate)
                );

                services.AddDefaultIdentity<SecurityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<SecurityRole>()
                    .AddEntityFrameworkStores<SecurityDbContext>();
            });
        }
    }
}