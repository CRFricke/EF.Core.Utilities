using System;

namespace CRFricke.EF.Core.Utilities
{
    /// <summary>
    /// Contains options for the initialization of a database.
    /// </summary>
    public class DbInitializerOption
    {
        /// <summary>
        /// Creates a new instance of the DbInitializerOption class with the specified options.
        /// </summary>
        /// <param name="dbContextType">
        /// The <see cref="Type"/> of the <see cref="DbContext"/> whose associated database is to be initialized.
        /// </param>
        /// <param name="dbInitializationOption">The database initialization option.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="dbContextType"/> parameter is <see langword="null"/>.
        /// </exception>
        public DbInitializerOption(Type dbContextType, DbInitializationOption dbInitializationOption)
        {
            DbContextType = dbContextType ?? throw new ArgumentNullException(nameof(dbContextType));
            DbInitializationOption = dbInitializationOption;
        }

        /// <summary>
        /// The <see cref="Type"/> of the <see cref="DbContext"/> whose associated database is to be initialized.
        /// </summary>
        public Type DbContextType { get; }

        /// <summary>
        /// The database initialization option.
        /// </summary>
        public DbInitializationOption DbInitializationOption { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"DbContextType='{DbContextType.FullName}', DbInitializationOption.{DbInitializationOption}";
        }
    }
}
