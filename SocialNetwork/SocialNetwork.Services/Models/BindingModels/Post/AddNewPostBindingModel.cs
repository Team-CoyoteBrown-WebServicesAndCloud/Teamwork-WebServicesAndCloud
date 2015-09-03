
namespace SocialNetwork.Services.Models.BindingModels.Post
{
    using System.ComponentModel.DataAnnotations;

    public class AddNewPostBindingModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public string Username { get; set; }
    }
}