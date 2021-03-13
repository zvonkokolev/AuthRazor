using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AuthRazor.Core;
using AuthRazor.Core.Contracts;
using AuthRazor.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthRazor.Basic.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [BindProperty]
        public string ReturnUrl { get; set; }

        [BindProperty]
        public string Message { get; set; }

        public IAuthUserRepository AuthUsers { get; }

        public LoginModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnGet(string message)
        {
            Message = message;
        }

        public async Task<ActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            AuthUser user = await _unitOfWork.AuthUsers.FindByEmailAsync(Email);

            // check if input password, after encryption, equals actually password
            if (user == null)
            {
                Message = "Benutzer nicht vorhanden!!!";
                return Page();
            }
            string enteredPassword = AuthUtils.GenerateHashedPassword(Password);
            if (!AuthUtils.VerifyPassword(enteredPassword, user.Password))
            {
                Message = "E-mail / Passwort stimmt nicht!!!";
                return Page();
            }

            if (user.UserRole == "Administrator")
            {
                return RedirectToPage("/AdministratorSettings", new { id = user.Id });
            }
            else if (user.UserRole == "User")
            {
                return RedirectToPage("/Settings", new { userId = user.Id });
            }
            else
            {
                Message = "Datenbank nicht erreichbar!!!";
                return Page();
            }
        }
    }
}
