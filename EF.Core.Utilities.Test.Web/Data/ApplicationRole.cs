using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace EF.Core.Utilities.Test.Web.Data
{
    public class ApplicationRole : IdentityRole
    {
        public virtual string Description { get; set; }

        public virtual ICollection<IdentityRoleClaim<string>> Claims { get; } = new List<IdentityRoleClaim<string>>();

        public ApplicationRole SetClaims(IEnumerable<string> claims) 
            => SetClaims(claims.ToArray());

        public ApplicationRole SetClaims(params string[] claims)
        {
            Claims.Clear();

            foreach (var claim in claims)
            {
                Claims.Add(CreateRoleClaim(roleId: Id, claimValue: claim));
            }

            return this;
        }

        private static IdentityRoleClaim<string> CreateRoleClaim(string roleId, string claimValue)
            => new IdentityRoleClaim<string> { RoleId = roleId, ClaimType = ClaimTypes.AuthorizationDecision, ClaimValue = claimValue };
    }
}
