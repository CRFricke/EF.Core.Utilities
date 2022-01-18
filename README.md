# EF.Core.Utilities

A set of utilities for use with Entity Framework Core.

## DbInitializer

Ensures that the database associated with the specified DbContext is created.
If the DbContext implements the `ISeedingContext` interface, its `SeedDatabaseAsync` method is called.

The DbInitializer is added to the IServiceCollection as a HostedService that runs as a background task 
before the application's request pipeline is started.

To configure the DbInitializer, use the `AddDbInitializer` IServiceCollection extension method in the 
ConfigureServices method of the Startup class.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"))
        );

    services.AddDbInitializer<ApplicationDbContext>();
    ⁝
}
```

## DbMigrator

Applies any pending migrations to the database associated with the specified DbContext.
If the database does not exist, it is created.
If the DbContext implements the `ISeedingContext` interface, its `SeedDatabaseAsync` method is called.

The DbMigrator is added to the IServiceCollection as a HostedService that will run before the application's 
request pipeline is started.

To configure the DbMigrator, use the `AddDbMigrator` IServiceCollection extension method in the 
ConfigureServices method of the Startup class.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"))
        );

    services.AddDbMigrator<ApplicationDbContext>();
    ⁝
}
```