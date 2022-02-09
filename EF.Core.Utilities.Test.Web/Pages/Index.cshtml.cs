using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace EF.Core.Utilities.Test.Web.Pages
{
    public class IndexModel : PageModel
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ILogger<IndexModel> _logger;
#pragma warning restore IDE0052 // Remove unread private members

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
