namespace SocialNetwork.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PhotoLike
    {
        public int Id { get; set; }

        public int PhotoId { get; set; }

        public virtual Photo Photo { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}