using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CRFricke.EF.Core.Utilities
{
    internal class DbInitializer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public DbInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var scopedProvider = scope.ServiceProvider;
            var logger = scopedProvider.GetRequiredService<ILoggerFactory>().CreateLogger<DbInitializer>();

            var options = scopedProvider.GetRequiredService<IOptions<DbInitializerOptions>>().Value;
            if (!options.Options.Any())
            {
                return;
            }

            var processedOptions = new List<DbInitializerOption>();

            foreach (var option in options.Options)
            {
                if (option.DbInitializationOption == DbInitializationOption.None)
                {
                    continue;
                }

                var processedOption = processedOptions.Find(o => o.DbContextType == option.DbContextType);
                if (processedOption != null)
                {
                    logger.LogWarning($"Duplicate DbContext entry ({option}); database already initialized using DbInitializationOption.{processedOption.DbInitializationOption}.");
                    continue;
                }

                processedOptions.Add(option);

                DbContext dbContext = (DbContext)scopedProvider.GetRequiredService(option.DbContextType);

                try
                {
                    switch (option.DbInitializationOption)
                    {
                        case DbInitializationOption.Migrate:
                            await dbContext.Database.MigrateAsync();
                            break;

                        case DbInitializationOption.EnsureCreated:
                            await dbContext.Database.EnsureCreatedAsync();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error occurred initializing database associated with DbContext '{option.DbContextType.FullName}'.");
                    continue;
                }

                if (dbContext is ISeedingContext seedingContext)
                {
                    try
                    {
                        await seedingContext.SeedDatabaseAsync(scopedProvider);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"Error occurred seeding database associated with DbContext '{option.DbContextType.FullName}'.");
                        continue;
                    }
                }

                logger.LogInformation($"Database associated with DbContext '{option.DbContextType.FullName}' initialized using DbInitializationOption.{option.DbInitializationOption}.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
