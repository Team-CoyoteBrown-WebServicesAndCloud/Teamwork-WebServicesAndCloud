using System.ComponentModel.DataAnnotations;
namespace SocialNetwork.Models
{
    public class CommentLike
    {
        public int Id { get; set; }

        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
         
    }
}