using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace EF.Core.Utilities.Test.Web.Areas.Identity.Data
{
    public class SecurityRole : IdentityRole
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SecurityRole"/> class
        /// </summary>
        public SecurityRole() : base()
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="SecurityRole"/> class with the specified name.
        /// </summary>
        /// <param name="roleName">The name of the new <see cref="SecurityRole"/>.</param>
        public SecurityRole(string roleName) : base(roleName)
        { }

        /// <summary>
        /// The claims that have been granted to this application role.
        /// </summary>
        public virtual ICollection<IdentityRoleClaim<string>> Claims { get; } = new List<IdentityRoleClaim<string>>();


        /// <summary>
        /// Sets the Claims collection of this <see cref="SecurityRole"/> object.
        /// </summary>
        /// <param name="role">The <see cref="SecurityRole"/> whose Claims collection is to be updated.</param>
        /// <param name="claims">The claim values to be assigned to this <see cref="SecurityRole"/>.</param>
        /// <returns>This <see cref="SecurityRole"/> instance.</returns>
        public SecurityRole SetClaims(params string[] claims)
        {
            Claims.Clear();

            foreach (var claim in claims)
            {
                Claims.Add(CreateRoleClaim(claim));
            }

            return this;
        }

        /// <summary>
        /// Creates a new IdentityRoleClaim using the specified ID and claim value.
        /// </summary>
        /// <param name="claimValue">The claim value.</param>
        /// <returns>A new IdentityRoleClaim with the specified values.</returns>
        private IdentityRoleClaim<string> CreateRoleClaim(string claimValue)
            => new IdentityRoleClaim<string> { RoleId = Id, ClaimType = ClaimTypes.AuthorizationDecision, ClaimValue = claimValue };
    }
}
