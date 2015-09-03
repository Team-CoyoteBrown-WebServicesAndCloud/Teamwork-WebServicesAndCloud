namespace SocialNetwork.Services.Models.BindingModels.Post
{
    using System.ComponentModel.DataAnnotations;

    public class EditPostBindingModel
    {
        [Required]
        public string Content { get; set; }
    }
}