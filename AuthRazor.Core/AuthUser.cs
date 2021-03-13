using System;
using System.ComponentModel.DataAnnotations;

namespace AuthRazor.Core
{
    public class AuthUser
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string UserRole { get; set; }
    }
}
