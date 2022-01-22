using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CRFricke.EF.Core.Utilities
{
    /// <summary>
    /// Contains the options for the DbInitializer service.
    /// </summary>
    public class DbInitializerOptions : List<DbInitializerOption>
    {
        /// <summary>
        /// Configures the DbInitializer to use the specified <see cref="DbContext"/> and <see cref="DbInitializationOption"/>.
        /// </summary>
        /// <typeparam name="TContext">
        /// The <see cref="Type"/> of the <see cref="DbContext"/> whose associated database is to be initialized.
        /// </typeparam>
        /// <param name="dbInitializationOption">The database initialization option.</param>
        /// <returns>The DbInitializerOptions instance so that further configuration can be chained.</returns>
        public DbInitializerOptions UseDbContext<TContext>(DbInitializationOption dbInitializationOption) where TContext: DbContext
        {
            Add(new DbInitializerOption(typeof(TContext), dbInitializationOption));

            return this;
        }

        /// <summary>
        /// Configures the DbInitializer to use the specified <see cref="DbContext"/> and <see cref="DbInitializationOption"/>.
        /// </summary>
        /// <param name="dbContextType">
        /// The <see cref="Type"/> of the <see cref="DbContext"/> whose associated database is to be initialized.
        /// </param>
        /// <param name="dbInitializationOption">The database initialization option.</param>
        /// <returns>The DbInitializerOptions instance so that further configuration can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="dbContextType"/> parameter is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the <paramref name="dbContextType"/> parameter specifies a Type that does not derive from <see cref="DbContext"/>.
        /// </exception>
        public DbInitializerOptions UseDbContext(Type dbContextType, DbInitializationOption dbInitializationOption)
        {
            if (dbContextType == null)
            {
                throw new ArgumentNullException(nameof(dbContextType));
            }

            if (!typeof(DbContext).IsAssignableFrom(dbContextType))
            {
                throw new ArgumentException($"'{dbContextType.Name}' does not derive from DbContext.", nameof(dbContextType));
            }

            Add(new DbInitializerOption(dbContextType, dbInitializationOption));

            return this;
        }
    }
}
