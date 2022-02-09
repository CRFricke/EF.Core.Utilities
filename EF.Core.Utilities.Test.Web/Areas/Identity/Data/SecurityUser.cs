using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;

namespace EF.Core.Utilities.Test.Web.Areas.Identity.Data
{
    public class SecurityUser : IdentityUser
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SecurityUser"/> class.
        /// </summary>
        public SecurityUser() : base()
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="SecurityUser"/> class with the specified email address.
        /// </summary>
        /// <param name="email">The email address of the new <see cref="SecurityUser"/>.</param>
        public SecurityUser(string email) : this(email, email)
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="SecurityUser"/> class with the specified email address and user name.
        /// </summary>
        /// <param name="email">The email address of the new <see cref="SecurityUser"/>.</param>
        /// <param name="userName">The user name of the new <see cref="SecurityUser"/>.</param>
        public SecurityUser(string email, string userName) : base(userName)
        {
            Email = email;
        }

        /// <summary>
        /// The claims that this application user possesses.
        /// </summary>
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; } = new List<IdentityUserClaim<string>>();


        /// <summary>
        /// Sets the Claims collection of this <see cref="SecurityUser"/> objects.
        /// </summary>
        /// <param name="user">The SecurityUser <see cref="SecurityUser"/> whose Claims collection is to be updated.</param>
        /// <param name="claims">The claim values to be assigned to this <see cref="SecurityUser"/>.</param>
        /// <returns>This <see cref="SecurityUser"/> instance.</returns>
        public SecurityUser SetClaims(params string[] claims)
        {
            Claims.Clear();

            foreach (var claim in claims)
            {
                Claims.Add(CreateUserClaim(claim));
            }

            return this;
        }

        /// <summary>
        /// Creates a new IdentityUserClaim using the specified Role ID and claim value.
        /// </summary>
        /// <param name="userId">The ID of the User being assigned the claim.</param>
        /// <param name="claimValue">The claim value.</param>
        private IdentityUserClaim<string> CreateUserClaim(string claimValue)
            => new() { UserId = Id, ClaimType = ClaimTypes.Role, ClaimValue = claimValue };
    }
}
