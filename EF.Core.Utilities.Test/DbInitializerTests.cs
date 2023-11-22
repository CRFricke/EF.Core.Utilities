using CRFricke.EF.Core.Utilities;
using CRFricke.Test.Support.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EF.Core.Utilities.Test;

public class DbInitializerTests
{
    [Fact(DisplayName = "Issues LogWarning when DbInitializerOptions is empty")]
    public async void Test01Async()
    {
        var dbInitializer = SetupTestEnvironment(
            new DbInitializerOptions(), 
            out Mock<DbContext> dbContext, out Mock<IMigrator> migrator, out Mock<DatabaseFacade> dbFacade, out FakeLogger<DbInitializer> logger);

        await dbInitializer.StartAsync(default);

        migrator.Verify(m => m.MigrateAsync(null, default), Times.Never());
        dbFacade.Verify(db => db.EnsureCreatedAsync(default), Times.Never());
        dbContext.As<ISeedingContext>().Verify(sc => sc.SeedDatabaseAsync(It.IsAny<IServiceProvider>()), Times.Never());

        Assert.Equal(1, logger.Collector.Count);
        Assert.Equal(LogLevel.Warning, logger.LatestRecord.Level);
        Assert.Contains("no DbContext entries", logger.LatestRecord.Message);
    }

    [Fact(DisplayName = "Issues LogWarning when DbInitializerOption is 'None'")]
    public async void Test02Async()
    {
        var dbInitializationOption = DbInitializationOption.None;
        var dbInitializer = SetupTestEnvironment(
            new DbInitializerOptions().UseDbContext<DbContext>(dbInitializationOption),
            out Mock<DbContext> dbContext, out Mock<IMigrator> migrator, out Mock<DatabaseFacade> dbFacade, out FakeLogger<DbInitializer> logger);

        await dbInitializer.StartAsync(default);

        migrator.Verify(m => m.MigrateAsync(null, default), Times.Never());
        dbFacade.Verify(db => db.EnsureCreatedAsync(default), Times.Never());
        dbContext.As<ISeedingContext>().Verify(sc => sc.SeedDatabaseAsync(It.IsAny<IServiceProvider>()), Times.Never());

        Assert.Equal(1, logger.Collector.Count);
        Assert.Equal(LogLevel.Warning, logger.LatestRecord.Level);
        Assert.Contains("skipping", logger.LatestRecord.Message);
    }

    [Fact(DisplayName = "Calls MigrateAsync when DbInitializerOption is 'Migrate'")]
    public async void Test03Async()
    {
        var dbInitializationOption = DbInitializationOption.Migrate;
        var dbInitializer = SetupTestEnvironment(
            new DbInitializerOptions().UseDbContext<DbContext>(dbInitializationOption),
            out Mock<DbContext> dbContext, out Mock<IMigrator> migrator, out Mock<DatabaseFacade> dbFacade, out FakeLogger<DbInitializer> logger);

        await dbInitializer.StartAsync(default);

        migrator.Verify(m => m.MigrateAsync(null, default), Times.Once());
        dbFacade.Verify(db => db.EnsureCreatedAsync(default), Times.Never());
        dbContext.As<ISeedingContext>().Verify(sc => sc.SeedDatabaseAsync(It.IsAny<IServiceProvider>()), Times.Once());

        Assert.Equal(1, logger.Collector.Count);
        Assert.Equal(LogLevel.Information, logger.LatestRecord.Level);
        Assert.Contains($"initialized using DbInitializationOption.{dbInitializationOption}", logger.LatestRecord.Message);
    }

    [Fact(DisplayName = "Calls EnsureCreatedAsync when DbInitializerOption is 'EnsureCreated'")]
    public async void Test04Async()
    {
        var dbInitializationOption = DbInitializationOption.EnsureCreated;
        var dbInitializer = SetupTestEnvironment(
            new DbInitializerOptions().UseDbContext<DbContext>(dbInitializationOption),
            out Mock<DbContext> dbContext, out Mock<IMigrator> migrator, out Mock<DatabaseFacade> dbFacade, out FakeLogger<DbInitializer> logger);

        await dbInitializer.StartAsync(default);

        migrator.Verify(m => m.MigrateAsync(null, default), Times.Never());
        dbFacade.Verify(db => db.EnsureCreatedAsync(default), Times.Once());
        dbContext.As<ISeedingContext>().Verify(sc => sc.SeedDatabaseAsync(It.IsAny<IServiceProvider>()), Times.Once());

        Assert.Equal(1, logger.Collector.Count);
        Assert.Equal(LogLevel.Information, logger.LatestRecord.Level);
        Assert.Contains($"initialized using DbInitializationOption.{dbInitializationOption}", logger.LatestRecord.Message);
    }

    [Fact(DisplayName = "Calls SeedDatabaseAsync when DbInitializerOption is 'SeedOnly'")]
    public async void Test05Async()
    {
        var dbInitializationOption = DbInitializationOption.SeedOnly;
        var dbInitializer = SetupTestEnvironment(
            new DbInitializerOptions().UseDbContext(typeof(DbContext), dbInitializationOption),
            out Mock<DbContext> dbContext, out Mock<IMigrator> migrator, out Mock<DatabaseFacade> dbFacade, out FakeLogger<DbInitializer> logger);

        await dbInitializer.StartAsync(default);

        migrator.Verify(m => m.MigrateAsync(null, default), Times.Never());
        dbFacade.Verify(db => db.EnsureCreatedAsync(default), Times.Never());
        dbContext.As<ISeedingContext>().Verify(sc => sc.SeedDatabaseAsync(It.IsAny<IServiceProvider>()), Times.Once());

        Assert.Equal(1, logger.Collector.Count);
        Assert.Equal(LogLevel.Information, logger.LatestRecord.Level);
        Assert.Contains($"initialized using DbInitializationOption.{dbInitializationOption}", logger.LatestRecord.Message);
    }

    [Fact(DisplayName = "Issues LogWarning when same DbContext is specified twice")]
    public async void Test06Async()
    {
        var dbInitializationOption = DbInitializationOption.SeedOnly;
        var dbInitializer = SetupTestEnvironment(
            new DbInitializerOptions()
                .UseDbContext(typeof(DbContext), dbInitializationOption)
                .UseDbContext(typeof(DbContext), DbInitializationOption.Migrate),
            out Mock<DbContext> dbContext, out Mock<IMigrator> migrator, out Mock<DatabaseFacade> dbFacade, out FakeLogger<DbInitializer> logger);

        await dbInitializer.StartAsync(default);

        migrator.Verify(m => m.MigrateAsync(null, default), Times.Never());
        dbFacade.Verify(db => db.EnsureCreatedAsync(default), Times.Never());
        dbContext.As<ISeedingContext>().Verify(sc => sc.SeedDatabaseAsync(It.IsAny<IServiceProvider>()), Times.Once());

        Assert.Equal(2, logger.Collector.Count);
        Assert.Equal(LogLevel.Warning, logger.LatestRecord.Level);
        Assert.Contains($"already initialized using DbInitializationOption.{dbInitializationOption}", logger.LatestRecord.Message);
    }

    [Fact(DisplayName = "Issues LogError when MigrateAsync fails")]
    public async void Test07Async()
    {
        var expectedException = new Exception("BOOM! Couldn't migrate database.");

        var dbInitializationOption = DbInitializationOption.Migrate;
        var dbInitializer = SetupTestEnvironment(
            new DbInitializerOptions().UseDbContext<DbContext>(dbInitializationOption),
            out Mock<DbContext> dbContext, out Mock<IMigrator> migrator, out Mock<DatabaseFacade> dbFacade, out FakeLogger<DbInitializer> logger, 
            dbException: expectedException
            );

        await dbInitializer.StartAsync(default);

        migrator.Verify(m => m.MigrateAsync(null, default), Times.Once());
        dbFacade.Verify(db => db.EnsureCreatedAsync(default), Times.Never());
        dbContext.As<ISeedingContext>().Verify(sc => sc.SeedDatabaseAsync(It.IsAny<IServiceProvider>()), Times.Never());

        Assert.Equal(1, logger.Collector.Count);
        Assert.Equal(LogLevel.Error, logger.LatestRecord.Level);
        Assert.NotNull(logger.LatestRecord.Exception);
        Assert.Equal(expectedException.Message, logger.LatestRecord.Exception.Message);
        Assert.Contains("Error occurred initializing database", logger.LatestRecord.Message);
    }

    [Fact(DisplayName = "Issues LogError when EnsureCreatedAsync fails")]
    public async void Test08Async()
    {
        var expectedException = new Exception("BOOM! Couldn't create database.");

        var dbInitializationOption = DbInitializationOption.EnsureCreated;
        var dbInitializer = SetupTestEnvironment(
            new DbInitializerOptions().UseDbContext<DbContext>(dbInitializationOption),
            out Mock<DbContext> dbContext, out Mock<IMigrator> migrator, out Mock<DatabaseFacade> dbFacade, out FakeLogger<DbInitializer> logger,
            dbException: expectedException
            );

        await dbInitializer.StartAsync(default);

        migrator.Verify(m => m.MigrateAsync(null, default), Times.Never());
        dbFacade.Verify(db => db.EnsureCreatedAsync(default), Times.Once());
        dbContext.As<ISeedingContext>().Verify(sc => sc.SeedDatabaseAsync(It.IsAny<IServiceProvider>()), Times.Never());

        Assert.Equal(1, logger.Collector.Count);
        Assert.Equal(LogLevel.Error, logger.LatestRecord.Level);
        Assert.NotNull(logger.LatestRecord.Exception);
        Assert.Equal(expectedException.Message, logger.LatestRecord.Exception.Message);
        Assert.Contains("Error occurred initializing database", logger.LatestRecord.Message);
    }

    [Fact(DisplayName = "Issues LogError when SeedDatabaseAsync fails")]
    public async void Test09Async()
    {
        var expectedException = new Exception("BOOM! Couldn't seed database.");

        var dbInitializationOption = DbInitializationOption.EnsureCreated;
        var dbInitializer = SetupTestEnvironment(
            new DbInitializerOptions().UseDbContext<DbContext>(dbInitializationOption),
            out Mock<DbContext> dbContext, out Mock<IMigrator> migrator, out Mock<DatabaseFacade> dbFacade, out FakeLogger<DbInitializer> logger,
            seedException: expectedException
            );

        await dbInitializer.StartAsync(default);

        migrator.Verify(m => m.MigrateAsync(null, default), Times.Never());
        dbFacade.Verify(db => db.EnsureCreatedAsync(default), Times.Once());
        dbContext.As<ISeedingContext>().Verify(sc => sc.SeedDatabaseAsync(It.IsAny<IServiceProvider>()), Times.Once());

        Assert.Equal(1, logger.Collector.Count);
        Assert.Equal(LogLevel.Error, logger.LatestRecord.Level);
        Assert.NotNull(logger.LatestRecord.Exception);
        Assert.Equal(expectedException.Message, logger.LatestRecord.Exception.Message);
        Assert.Contains("Error occurred seeding database", logger.LatestRecord.Message);
    }


    /// <summary>
    /// Performs common setup and returns an instance of the DbInitializer class.
    /// </summary>
    /// <param name="options">The <see cref="DbInitializerOptions"/> to be configured.</param>
    /// <param name="dbContextMock">An output parameter containing the DbContext Mock object.</param>
    /// <param name="migratorMock">An output parameter containing the IMigrator Mock object.</param>
    /// <param name="dbFacadeMock">An output parameter containing the DatabaseFacade Mock object.</param>
    /// <param name="fakeLogger">An output parameter containing the FakeLogger object.</param>
    /// <returns>The new DbInitializer instance.</returns>
    private static DbInitializer SetupTestEnvironment(DbInitializerOptions options,  
        out Mock<DbContext> dbContextMock, out Mock<IMigrator> migratorMock, out Mock<DatabaseFacade> dbFacadeMock, out FakeLogger<DbInitializer> fakeLogger,
        Exception dbException = null, Exception seedException = null
        )
    {
        var logger = new FakeLogger<DbInitializer>();

        var migrator = new Mock<IMigrator>();
        if (dbException == null) {
            migrator.Setup(m => m.MigrateAsync(null, default)).Returns(Task.CompletedTask);
        } 
        else {
            migrator.Setup(m => m.MigrateAsync(null, default)).Throws(dbException);
        } 

        var dbContext = new Mock<DbContext>();
        dbContext.As<IInfrastructure<IServiceProvider>>().Setup(sp => sp.Instance).Returns(
            Mock.Of<IServiceProvider>(sp => sp.GetService(typeof(IMigrator)) == migrator.Object)
            );

        if (seedException == null) {
            dbContext.As<ISeedingContext>().Setup(sc => sc.SeedDatabaseAsync(It.IsAny<IServiceProvider>())).Returns(Task.CompletedTask);
        }
        else {
            dbContext.As<ISeedingContext>().Setup(sc => sc.SeedDatabaseAsync(It.IsAny<IServiceProvider>())).Throws(seedException);
        }

        var dbFacade = new Mock<DatabaseFacade>(dbContext.Object);
        if (dbException == null) {
            dbFacade.Setup(db => db.EnsureCreatedAsync(default)).Returns(Task.FromResult(true));
        }
        else {
            dbFacade.Setup(db => db.EnsureCreatedAsync(default)).Throws(dbException);
        }

        dbContext.Setup(c => c.Database).Returns(dbFacade.Object);

        var scopedProvider = Mock.Of<IServiceProvider>(
            sp => sp.GetService(typeof(ILoggerFactory)) ==
                Mock.Of<ILoggerFactory>(lf => lf.CreateLogger(typeof(DbInitializer).FullName) == logger) &&
            sp.GetService(typeof(IOptions<DbInitializerOptions>)) ==
                Mock.Of<IOptions<DbInitializerOptions>>(o => o.Value == options) &&
            sp.GetService(typeof(DbContext)) == dbContext.Object
            );

        var serviceProvider =
            Mock.Of<IServiceProvider>(sp => sp.GetService(typeof(IServiceScopeFactory)) ==
                Mock.Of<IServiceScopeFactory>(ssf => ssf.CreateScope() ==
                    Mock.Of<IServiceScope>(ss => ss.ServiceProvider == scopedProvider
                    )
                )
            );

        dbContextMock = dbContext;
        migratorMock = migrator;
        dbFacadeMock = dbFacade;
        fakeLogger = logger;

        return new DbInitializer(serviceProvider);
    }
}