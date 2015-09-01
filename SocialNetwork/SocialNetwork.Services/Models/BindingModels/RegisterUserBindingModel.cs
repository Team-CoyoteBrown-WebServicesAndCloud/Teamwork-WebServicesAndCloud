namespace SocialNetwork.Services.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using SocialNetwork.Models.Enum;

    public class RegisterUserBindingModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]

        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(0, 100)]
        public int? Age { get; set; }

        public Gender Gender { get; set; }
    }
}