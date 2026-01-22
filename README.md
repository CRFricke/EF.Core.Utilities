![EF.Core.Utilities](https://raw.githubusercontent.com/CRFricke/EF.Core.Utilities/master/tools.png)
[![.NET](https://github.com/CRFricke/EF.Core.Utilities/actions/workflows/dotnet.yml/badge.svg)](https://github.com/CRFricke/EF.Core.Utilities/actions/workflows/dotnet.yml)

# EF.Core.Utilities

A set of utilities for use with Entity Framework Core.

## DbInitializer

The DbInitializer initializes the database associated with a specified DbContext. 
It runs before the application's request pipeline is started.
If the DbContext implements the `ISeedingContext` interface, its `SeedDatabaseAsync` method is also called.

```csharp
/// <summary>
/// Identifies a self seeding database context. 
/// </summary>
public interface ISeedingContext
{
    /// <summary>
    /// Called to seed the database.
    /// </summary>
    /// <param name="serviceProvider">
    /// An <see cref="IServiceProvider"/> instance for accessing support services.
    /// </param>
    /// <returns>
    /// A Task instance that can be used to await completion of the method.
    /// </returns>
    Task SeedDatabaseAsync(IServiceProvider serviceProvider);
}
```

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
        options.UseDbContext<ApplicationDbContext>(DbInitializationOption.Migrate);
            });
    ⁝
}
```

More than one `UseDbContext` clause can be chained if the application contains more than one database.

[Installation icons created by catkuro - Flaticon](https://www.flaticon.com/free-icons/installation "installation icons")