using Microsoft.AspNetCore.Identity;

namespace EF.Core.Utilities.Test.Web.Data
{
    public class ApplicationRole : IdentityRole
    {
        public virtual string Description { get; set; }
    }
}
