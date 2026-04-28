using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CRFricke.EF.Core.Utilities;

#pragma warning disable CA1031 // Do not catch general exception types

internal partial class DbInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DbInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    #region Log Messages

    [LoggerMessage(EventId = 1, Level = LogLevel.Warning, Message = "DbInitializerOptions contains no DbContext entries.")]
    static partial void LogNoDbContextEntries(ILogger logger);

    [LoggerMessage(EventId = 2, Level = LogLevel.Warning, Message = "DbInitializationOption.{DbInitializationOption} specified for DbContext '{DbContextType}'; skipping.")]
    static partial void LogSkippingDbContext(ILogger logger, DbInitializationOption dbInitializationOption, string? dbContextType);

    [LoggerMessage(EventId = 3, Level = LogLevel.Warning, Message = "Duplicate DbContext entry ({DbInitializerOption}); database already initialized using DbInitializationOption.{ProcessedDbInitializationOption}.")]
    static partial void LogDuplicateDbContext(ILogger logger, DbInitializerOption dbInitializerOption, DbInitializationOption processedDbInitializationOption);

    [LoggerMessage(EventId = 4, Level = LogLevel.Error, Message = "Error occurred initializing database associated with DbContext '{DbContextType}'.")]
    static partial void LogInitializationError(ILogger logger, Exception ex, string? dbContextType);

    [LoggerMessage(EventId = 5, Level = LogLevel.Error, Message = "Error occurred seeding database associated with DbContext '{DbContextType}'.")]
    static partial void LogSeedingError(ILogger logger, Exception ex, string? dbContextType);

    [LoggerMessage(EventId = 6, Level = LogLevel.Information, Message = "Database associated with DbContext '{DbContextType}' initialized using DbInitializationOption.{DbInitializationOption}.")]
    static partial void LogDatabaseInitialized(ILogger logger, string? dbContextType, DbInitializationOption dbInitializationOption);

    #endregion

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var scopedProvider = scope.ServiceProvider;
        var logger = scopedProvider.GetRequiredService<ILoggerFactory>().CreateLogger<DbInitializer>();

        var options = scopedProvider.GetRequiredService<IOptions<DbInitializerOptions>>().Value;
        if (!options.Any())
        {
            LogNoDbContextEntries(logger);
            return;
        }

        var processedOptions = new List<DbInitializerOption>();

        foreach (var option in options)
        {
            if (option.DbInitializationOption == DbInitializationOption.None)
            {
                LogSkippingDbContext(logger, option.DbInitializationOption, option.DbContextType.FullName);
                continue;
            }

            var processedOption = processedOptions.Find(o => o.DbContextType == option.DbContextType);
            if (processedOption != null)
            {
                LogDuplicateDbContext(logger, option, processedOption.DbInitializationOption);
                continue;
            }

            processedOptions.Add(option);

            DbContext dbContext = (DbContext)scopedProvider.GetRequiredService(option.DbContextType);

            try
            {
                switch (option.DbInitializationOption)
                {
                    case DbInitializationOption.Migrate:
                        await dbContext.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
                        break;

                    case DbInitializationOption.EnsureCreated:
                        await dbContext.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
                        break;
                }
            }
            catch (Exception ex)
            {
                LogInitializationError(logger, ex, option.DbContextType.FullName);
                continue;
            }

            if (dbContext is ISeedingContext seedingContext)
            {
                try
                {
                    await seedingContext.SeedDatabaseAsync(scopedProvider).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    LogSeedingError(logger, ex, option.DbContextType.FullName);
                    continue;
                }
            }

            LogDatabaseInitialized(logger, option.DbContextType.FullName, option.DbInitializationOption);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
