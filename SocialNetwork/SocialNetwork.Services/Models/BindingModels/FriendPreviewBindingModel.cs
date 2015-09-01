namespace SocialNetwork.Services.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class FriendPreviewBindingModel
    {
        [Required]
        public string Name { get; set; }
    }
}