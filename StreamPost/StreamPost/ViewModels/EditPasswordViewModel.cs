using System.ComponentModel.DataAnnotations;

namespace StreamPost.ViewModels
{
    public class EditPasswordViewModel
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must match.")]
        public string ConfirmPassword { get; set; }
    }
}
