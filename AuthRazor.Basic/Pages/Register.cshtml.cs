using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AuthRazor.Core;
using AuthRazor.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthRazor.Basic.Pages
{
    public class RegisterModel : PageModel
    {
        private IUnitOfWork _unitOfWork;

        [BindProperty]
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [BindProperty]
        [Compare(nameof(Password), ErrorMessage = "Confirm password not equal password")]
        [Required(ErrorMessage = "Confirm Password is required")]
		[Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

		[BindProperty]
		public string ReturnUrl { get; set; }

		[BindProperty]
		public string Message { get; set; }

        public RegisterModel(IUnitOfWork unitOfWork)
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

			if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword))
			{
				Message = "Falsche Eingaben";
				return Page();
			}
			if (!Password.Equals(ConfirmPassword))
			{
				Message = "Das Passwort und die Bestätigung stimmen nicht überein.";
				return Page();
			}

			// Password encrypting
			string hashedPassword = Utils.AuthUtils.GenerateHashedPassword(Password);

			// User initialize
			AuthUser user = new AuthUser()
            {
				Email = Email,
				Password = hashedPassword,
				UserRole = "User"
			};
			await _unitOfWork.AuthUsers.AddUserAsync(user);
			try
			{
				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception e)
			{
				Message = $"{e.InnerException.Message.ToLowerInvariant()}";
				return Page();
			}

			Message = "Danke für Ihre Anmeldung";
			return RedirectToPage("/Login", new { Message });
		}
	}
}
