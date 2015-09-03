namespace SocialNetwork.Services.Models.BindingModels.Post
{
    using System.ComponentModel.DataAnnotations;

    public class DeletePostBindingModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public string WallOwnerId { get; set; }
    }
}