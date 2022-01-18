# EF.Core.Utilities

A set of utilities for use with Entity Framework Core.

## DbInitializer

Initializes the database associated with the specified DbContext. 
If the DbContext implements the `ISeedingContext` interface, its `SeedDatabaseAsync` method is also called.
The DbInitializer runs before the application's request pipeline is started.

The type of initialization is controlled by the specified `DbInitializationOption` value.

```csharp
public enum DbInitializationOption
{
    /// <summary>
    /// No initialization is to be performed.
    /// </summary>
    None = 0,

    /// <summary>
    /// Ensure the database is created and seed the database.
    /// </summary>
    EnsureCreated = 1,

    /// <summary>
    /// Apply pending migrations and seed the database.
    /// </summary>
    Migrate = 2,

    /// <summary>
    /// Only seed the database.
    /// </summary>
    SeedOnly = 3
}
```

To configure the DbInitializer, use the `AddDbInitializer` IServiceCollection extension method (in the 
ConfigureServices method of the Startup class).

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
        );

    services.AddDbInitializer(options => {
        options.UseDbContext(typeof(ApplicationDbContext), DbInitializationOption.Migrate);
            });
    ⁝
}
```

<a href="https://www.flaticon.com/free-icons/installation" title="installation icons">Installation icons created by catkuro - Flaticon</a>