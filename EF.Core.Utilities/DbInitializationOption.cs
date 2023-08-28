namespace CRFricke.EF.Core.Utilities;

/// <summary>
/// Provides the database initialization option for the DbInitializer.
/// </summary>
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
