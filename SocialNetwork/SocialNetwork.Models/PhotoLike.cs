using System.ComponentModel.DataAnnotations;
namespace SocialNetwork.Models
{
    public class PhotoLike
    {
        public int Id { get; set; }

        //I'm not so shure about the comments ?!
        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [Required]
        public string PhotoOwnerId { get; set; }

        public virtual ApplicationUser PhotoOwner { get; set; }

    }
}