using System;
using System.Threading.Tasks;

namespace CRFricke.EF.Core.Utilities;

/// <summary>
/// Identifies a self seeding database context. 
/// </summary>
public interface ISeedingContext
{
    /// <summary>
    /// Called to seed the database.
    /// </summary>
    /// <param name="serviceProvider">An <see cref="IServiceProvider"/> instance for accessing support services.</param>
    /// <returns>A Task instance that can be used to await completion of the method.</returns>
    Task SeedDatabaseAsync(IServiceProvider serviceProvider);
}
