using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CRFricke.EF.Core.Utilities
{
    /// <summary>
    /// Initializes the database associated with <typeparamref name="TContext"/> during application startup. 
    /// </summary>
    /// <typeparam name="TContext">The Type of the DbContext to be initialized.</typeparam>
    /// <remarks>
    /// If <typeparamref name="TContext"/> implements the <see cref="ISeedingContext"/> interface, its SeedDatabaseAsync method will be called.
    /// </remarks>
    internal class DbInitializer<TContext> : IHostedService where TContext : DbContext
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Creates a new instance of the DbInitializer class with the specified parameters.
        /// </summary>
        /// <param name="serviceProvider">An <see cref="IServiceProvider"/> instance for accessing support services.</param>
        public DbInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var scopedProvider = scope.ServiceProvider;
            var dbContext = scopedProvider.GetRequiredService<TContext>();

            try
            {
                await dbContext.Database.EnsureCreatedAsync();
            }
            catch (Exception ex)
            {
                var logger = scopedProvider.GetRequiredService<ILoggerFactory>().CreateLogger<DbInitializer<TContext>>();
                logger.LogError(ex, "Database.EnsureCreatedAsync() failed.");
                return;
            }

            if (dbContext is ISeedingContext seedingContext)
            {
                try
                {
                    await seedingContext.SeedDatabaseAsync(scopedProvider);
                }
                catch (Exception ex)
                {
                    var logger = scopedProvider.GetRequiredService<ILoggerFactory>().CreateLogger<DbInitializer<TContext>>();
                    logger.LogError(ex, "SeedDatabaseAsync() failed.");
                }
            }
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
