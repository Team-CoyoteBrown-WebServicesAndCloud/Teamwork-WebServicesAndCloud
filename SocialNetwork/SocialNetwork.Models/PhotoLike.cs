namespace SocialNetwork.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PhotoLike
    {
        public int Id { get; set; }

        public int PhotoId { get; set; }

        public virtual Photo Photo { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [Required]
        public string PhotoOwnerId { get; set; }

        public virtual ApplicationUser PhotoOwner { get; set; }

    }
}