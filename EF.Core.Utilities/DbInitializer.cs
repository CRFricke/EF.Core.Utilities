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
            if (!options.Any())
            {
                logger.LogWarning("DbInitializerOptions contains no DbContext entries.");
                return;
            }

            var processedOptions = new List<DbInitializerOption>();

            foreach (var option in options)
            {
                if (option.DbInitializationOption == DbInitializationOption.None)
                {
                    logger.LogWarning(
                        "DbInitializationOption.{DbInitializationOption} specified for DbContext '{DbContextType}'; skipping.",
                        option.DbInitializationOption, option.DbContextType.FullName
                        );
                    continue;
                }

                var processedOption = processedOptions.Find(o => o.DbContextType == option.DbContextType);
                if (processedOption != null)
                {
                    logger.LogWarning(
                        "Duplicate DbContext entry ({DbInitializerOption}); database already initialized using DbInitializationOption.{DbInitializationOption}.", 
                        option, processedOption.DbInitializationOption
                        );
                    continue;
                }

                processedOptions.Add(option);

                DbContext dbContext = (DbContext)scopedProvider.GetRequiredService(option.DbContextType);

                try
                {
                    switch (option.DbInitializationOption)
                    {
                        case DbInitializationOption.Migrate:
                            await dbContext.Database.MigrateAsync(cancellationToken);
                            break;

                        case DbInitializationOption.EnsureCreated:
                            await dbContext.Database.EnsureCreatedAsync(cancellationToken);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred initializing database associated with DbContext '{DbContextType}'.", 
                        option.DbContextType.FullName
                        );
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
                        logger.LogError(ex, "Error occurred seeding database associated with DbContext '{DbContextType}'.", 
                            option.DbContextType.FullName
                            );
                        continue;
                    }
                }

                logger.LogInformation("Database associated with DbContext '{DbContextType}' initialized using DbInitializationOption.{DbInitializationOption}.", 
                    option.DbContextType.FullName, option.DbInitializationOption
                    );
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
