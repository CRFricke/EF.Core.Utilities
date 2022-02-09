using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace EF.Core.Utilities.Test.Web.Pages
{
    public class PrivacyModel : PageModel
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ILogger<PrivacyModel> _logger;
#pragma warning restore IDE0052 // Remove unread private members

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
