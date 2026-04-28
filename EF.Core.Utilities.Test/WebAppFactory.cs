using CRFricke.Test.Support.Infrastructure;
using EF.Core.Utilities.Test.Web.Areas.Identity;
using EF.Core.Utilities.Test.Web.Data;
using Microsoft.Extensions.Hosting;

#pragma warning disable CA1515 // Consider making public types internal

namespace EF.Core.Utilities.Test;

public class WebAppFactory : WebApplicationFactory<IdentityHostingStartup, ApplicationDbContext>
{
    public override string HostingEnvironment => Environments.Development;

    public override Uri HostUri =>  new("http://EfCoreUtilities.dev.localhost");
}
