using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthRazor.Basic.Pages
{
    [Authorize(Roles = "User")]
    public class SettingsModel : PageModel
    {
        public ActionResult OnGet()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Login", "Sie sind abgemeldet");
            }
            return Page();
        }
    }
}
