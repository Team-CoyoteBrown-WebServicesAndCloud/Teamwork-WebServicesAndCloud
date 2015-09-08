namespace SocialNetwork.Services.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class WallBindingModel
    {
        [Required]
        public int PostsCount { get; set; }

        [Required]
        public int StartPostNumber { get; set; }

        [Required]
        public string Username { get; set; }
    }
}