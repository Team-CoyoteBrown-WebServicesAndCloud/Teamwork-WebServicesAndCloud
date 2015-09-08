namespace SocialNetwork.Services.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class NewsFeedBindingModel
    {
        [Required]
        public int PostsCount { get; set; }

        [Required]
        public int StartPostNumber { get; set; }
    }
}