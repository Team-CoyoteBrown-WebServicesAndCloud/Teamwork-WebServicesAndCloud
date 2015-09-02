
namespace SocialNetwork.Services.Models.BindingModels.Post
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class BasePostBindingModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime PostedOn { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public string WallOwnerId { get; set; }
    }
}