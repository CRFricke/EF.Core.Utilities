using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CRFricke.EF.Core.Utilities
{
    /// <summary>
    /// Extension methods for setting up the <see cref="DbInitializer"/> service in an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a DbInitializer that will initialize the database(s) specified by <paramref name="optionsAction"/> parameter during application startup.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="optionsAction">The action to configure the options for the DbInitializer.</param>
        /// <returns>The same <see cref="IServiceProvider"/> instance so multiple calls can be chained.</returns>
        public static IServiceCollection AddDbInitializer(this IServiceCollection services, Action<DbInitializerOptions> optionsAction)
        {
            services.Configure(optionsAction);

            // If the AspNetCore 6.0 minimal bootstrap API is being used, ServiceDescriptors for hosted services are stored seperately 
            // from the rest of the ServiceDescriptors. In this case, they reside in an internal "HostedServices" property.

            IList<ServiceDescriptor> collection = services;

            if (services.GetType().FullName == "Microsoft.AspNetCore.WebApplicationServiceCollection")
            {
                var pi = services.GetType().GetProperty("HostedServices");
                if (pi != null)
                {
                    collection = (IList<ServiceDescriptor>)pi.GetValue(services)!;
                }
            }

            if (!collection.Any(sd => sd.ServiceType == typeof(IHostedService) && sd.ImplementationType == typeof(DbInitializer)))
            {
                services.AddHostedService<DbInitializer>();
            }

            return services;
        }
    }
}
