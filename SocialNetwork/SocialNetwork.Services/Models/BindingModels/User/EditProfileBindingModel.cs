namespace SocialNetwork.Services.Models.BindingModels.User
{
    using System.ComponentModel.DataAnnotations;
    using SocialNetwork.Models.Enum;

    public class EditProfilBindingModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public Gender? Gender { get; set; }

        public string ProfileImageData { get; set; }

        public string CoverImageData { get; set; }
    }
}