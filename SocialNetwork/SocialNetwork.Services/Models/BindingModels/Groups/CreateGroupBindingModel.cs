namespace SocialNetwork.Services.Models.BindingModels.Groups
{
    using System.ComponentModel.DataAnnotations;
    using SocialNetwork.Models.Enum;

    public class CreateGroupBindingModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string CoverImageData { get; set; }

        [Required]
        public GroupType Type { get; set; }
    }
}