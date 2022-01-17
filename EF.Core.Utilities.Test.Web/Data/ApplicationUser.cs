using Microsoft.AspNetCore.Identity;

namespace EF.Core.Utilities.Test.Web.Data
{
    public class ApplicationUser : IdentityUser
    {
        public virtual string GivenName { get; set; }

        public virtual string Surname { get; set; }
    }
}
