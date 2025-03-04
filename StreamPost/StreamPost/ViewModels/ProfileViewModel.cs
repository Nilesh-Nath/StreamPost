using System.ComponentModel.DataAnnotations;

namespace StreamPost.ViewModels
{
    public class ProfileViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string DateOfBirth { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

       
    }
}
