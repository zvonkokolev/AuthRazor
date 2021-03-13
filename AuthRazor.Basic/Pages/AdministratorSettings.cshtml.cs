using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthRazor.Basic.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdministratorSettingsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
