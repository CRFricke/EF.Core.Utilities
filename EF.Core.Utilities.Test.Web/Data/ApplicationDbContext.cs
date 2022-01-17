using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EF.Core.Utilities.Test.Web.Data
{
    public class ApplicationDbContext : ApplicationDbContext<ApplicationUser, ApplicationRole>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
    }

    public class ApplicationDbContext<TUser, TRole> : IdentityDbContext<TUser, TRole, string> 
        where TRole : IdentityRole, new()
        where TUser : IdentityUser, new()
    {
        protected ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext<TUser, TRole>> options) : base(options)
        { }
    }
}
