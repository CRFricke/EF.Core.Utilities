using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CRFricke.EF.Core.Utilities;

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
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2075:DynamicallyAccessedMembers", Justification = "Just accessing 'HostedServices' property of passed IServiceCollection implementation.")]
    public static IServiceCollection AddDbInitializer(this IServiceCollection services, Action<DbInitializerOptions> optionsAction)
    {
        services.Configure(optionsAction);

        // In AspNetCore 6.0, ServiceDescriptors for hosted services are stored separately, in the "HostedServices" 
        // property of the class that implements the IServiceCollection interface (WebApplicationServiceCollection).

        IList<ServiceDescriptor> collection = services;

        var pi = services.GetType().GetProperty("HostedServices");
        if (pi != null)
        {
            collection = (IList<ServiceDescriptor>)pi.GetValue(services)!;
        }

        if (!collection.Any(sd => sd.ServiceType == typeof(IHostedService) && sd.ImplementationType == typeof(DbInitializer)))
        {
            services.AddHostedService<DbInitializer>();
        }

        return services;
    }
}
