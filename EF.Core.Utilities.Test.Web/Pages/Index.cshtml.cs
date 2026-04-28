using Microsoft.AspNetCore.Mvc.RazorPages;

#pragma warning disable CA1515 // Consider making public types internal
#pragma warning disable IDE0052 // Remove unread private members

namespace EF.Core.Utilities.Test.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
