using System.ComponentModel.DataAnnotations;

namespace StreamPost.ViewModels
{
    public class SignupViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        [Required]
        public char Gender { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
        [Required]
        public string Password {  get; set; }
        [Required]
        [Compare("Password",ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
