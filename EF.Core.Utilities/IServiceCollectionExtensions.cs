using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CRFricke.EF.Core.Utilities
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a DbInitializer that will initialize the database associated with <typeparamref name="TContext"/> during application startup.
        /// </summary>
        /// <typeparam name="TContext">The Type of the DbContext whose database is to be initialized.</typeparam>
        /// <param name="serviceProvider">An <see cref="IServiceProvider"/> instance for accessing support services.</param>
        /// <returns>The same <see cref="IServiceProvider"/> instance so multiple calls can be chained.</returns>
        /// <remarks>
        /// If <typeparamref name="TContext"/> implements the <see cref="ISeedingContext"/> interface, its SeedDatabaseAsync method will be called.
        /// </remarks>
        public static IServiceCollection AddDbInitializer<TContext>(this IServiceCollection services) where TContext: DbContext
        {
            services.AddHostedService<DbInitializer<TContext>>();
            return services;
        }

        /// <summary>
        /// Registers a DbMigrator that will initialize the database associated with <typeparamref name="TContext"/> 
        /// and run any outstanding migrations during application startup.
        /// </summary>
        /// <typeparam name="TContext">The Type of the DbContext whose database is to be migrated.</typeparam>
        /// <param name="serviceProvider">An <see cref="IServiceProvider"/> instance for accessing support services.</param>
        /// <returns>The same <see cref="IServiceProvider"/> instance so multiple calls can be chained.</returns>
        /// <remarks>
        /// If <typeparamref name="TContext"/> implements the <see cref="ISeedingContext"/> interface, its SeedDatabaseAsync method will be called.
        /// </remarks>
        public static IServiceCollection AddDbMigrator<TContext>(this IServiceCollection services) where TContext: DbContext
        {
            services.AddHostedService<DbMigrator<TContext>>();
            return services;
        }
    }
}
