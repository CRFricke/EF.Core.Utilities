using CRFricke.Test.Support.Infrastructure;
using EF.Core.Utilities.Test.Web.Areas.Identity;
using EF.Core.Utilities.Test.Web.Data;
using Microsoft.Extensions.Hosting;

namespace EF.Core.Utilities.Test;

public class WebAppFactory : WebApplicationFactory<IdentityHostingStartup, ApplicationDbContext>
{
    public override string HostingEnvironment => Environments.Development;

    public override string HostUrl => "http://EfCoreUtilities.dev.localhost";
}
