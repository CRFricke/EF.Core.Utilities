using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace CRFricke.EF.Core.Utilities
{
    /// <summary>
    /// Extension methods for setting up the <see cref="DbInitializer"/> service in an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        ///// <summary>
        ///// Registers a DbInitializer that will initialize the database associated with <typeparamref name="TContext"/> during application startup.
        ///// </summary>
        ///// <typeparam name="TContext">The Type of the DbContext whose database is to be initialized.</typeparam>
        ///// <param name="serviceProvider">An <see cref="IServiceProvider"/> instance for accessing support services.</param>
        ///// <returns></returns>
        /// <summary>
        /// Registers a DbInitializer that will initialize the database(s) specified by <paramref name="optionsAction"/> parameter during application startup.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="optionsAction">The action to configure the options for the DbInitializer.</param>
        /// <returns>The same <see cref="IServiceProvider"/> instance so multiple calls can be chained.</returns>
        public static IServiceCollection AddDbInitializer(this IServiceCollection services, Action<DbInitializerOptions> optionsAction)
        {
            services.Configure(optionsAction);

            if (!services.Any(sd => sd.ServiceType == typeof(IHostedService) && sd.ImplementationType == typeof(DbInitializer)))
            {
                services.AddHostedService<DbInitializer>();
            }

            return services;
        }
    }
}
