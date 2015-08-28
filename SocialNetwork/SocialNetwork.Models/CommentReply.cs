using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SocialNetwork.Models
{
    public class CommentReply
    {
        private ICollection<CommentLike> likes;

        public CommentReply()
        {
            this.likes = new HashSet<CommentLike>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        public string Content { get; set; }

        public DateTime RepliedOn { get; set; }

        [Required]
        public int AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [Required]
        public int WallOwnerId { get; set; }

        public virtual ApplicationUser WallOwner { get; set; }

        public virtual ICollection<CommentLike> Likes
        {
            get { return this.likes; }
            set { this.likes = value; }
        }

    }
}