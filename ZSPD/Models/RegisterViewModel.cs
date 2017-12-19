using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZSPD.Domain.Models.EntityModels.Accounts;

namespace ZSPD.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Nazwa użytkownika")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public List<string> UserRoles
        {
            get
            {
                return new List<string>()
                {
                    "Sędzia",
                    "Psycholog",
                    "Student"
                };
            }
        }

        public string SelectedRole { get; set; }
    }
}