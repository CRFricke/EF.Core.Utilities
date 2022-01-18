using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace EF.Core.Utilities.Test.Web.Data
{
    public class ApplicationUser : IdentityUser
    {
        public virtual string GivenName { get; set; }

        public virtual string Surname { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; } = new List<IdentityUserClaim<string>>();

        public ApplicationUser SetClaims(IEnumerable<string> claims)
            => SetClaims(claims.ToArray());

        public ApplicationUser SetClaims(params string[] claims)
        {
            Claims.Clear();

            foreach (var claim in claims)
            {
                Claims.Add(CreateUserClaim(userId: Id, claimValue: claim));
            }

            return this;
        }

        private static IdentityUserClaim<string> CreateUserClaim(string userId, string claimValue)
            => new IdentityUserClaim<string> { UserId = userId, ClaimType = ClaimTypes.Role, ClaimValue = claimValue };
    }
}
