using System.Collections.Generic;

namespace EF.Core.Utilities.Test.Web.Areas.Identity.Data
{
    public class SecurityGuids
    {
        /// <summary>
        /// The Guids of the system <see cref="SecurityRole">SecurityRoles</see>.
        /// </summary>
        public class Role
        {
            /// <summary>
            /// The Guid assigned to the Administrator SecurityRole.
            /// </summary>
            public const string Administrator = "39dbc29b-c42c-4d72-a923-64a34a819e4f";
        }

        /// <summary>
        /// The Guids of the system Users.
        /// </summary>
        public class User
        {
            /// <summary>
            /// The Guid assigned to the Administrator <see cref="SecurityUser"/> account.
            /// </summary>
            public const string Administrator = "c8121441-032f-4975-aa2a-d9f0a1ce7a61";
        }
    }
}
