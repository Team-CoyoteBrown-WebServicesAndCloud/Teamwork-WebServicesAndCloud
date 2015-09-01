namespace SocialNetwork.Services.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using SocialNetwork.Models.Enum;

    public class EditProfilBindingModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public Gender Gender { get; set; }
    }
}