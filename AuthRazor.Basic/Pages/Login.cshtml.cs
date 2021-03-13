using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthRazor.Core;
using AuthRazor.Core.Contracts;
using AuthRazor.Utils;
using Microsoft.AspNetCore.Authentication;
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
            
            if (!AuthUtils.VerifyPassword(Password, user.Password))
            {
                Message = "E-mail / Passwort stimmt nicht!!!";
                return Page();
            }
            // prepare cookie 
            var authClaim = new List<Claim> 
            {
                new Claim(ClaimTypes.Email, user.Email)
            };
            if (!string.IsNullOrEmpty(user.UserRole))
            {
                authClaim.Add(new Claim(ClaimTypes.Role, user.UserRole));
            }
            var identity = new ClaimsIdentity(authClaim, "AuthUserIdentity");
            var userPrincipal = new ClaimsPrincipal(new[] { identity});
            // create cookie
            await HttpContext.SignInAsync(userPrincipal);

            if (user.UserRole == "Administrator")
            {
                return RedirectToPage("/AdministratorSettings", new { id = user.Id });
                //return RedirectToPage(Request.Query["ReturnUrl"]);
            }
            else if (user.UserRole == "User")
            {
                return RedirectToPage("/Settings", new { userId = user.Id });
                //return RedirectToPage(Request.Query["ReturnUrl"]);
            }
            else
            {
                Message = "Datenbank nicht erreichbar!!!";
                return Page();
            }
        }
    }
}
