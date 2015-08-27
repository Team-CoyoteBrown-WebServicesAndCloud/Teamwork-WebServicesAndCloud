using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SocialNetwork.Models
{
    public class CommentReply
    {
        private ICollection<CommentLike> commentLikes;

        public CommentReply()
        {
            this.commentLikes = new HashSet<CommentLike>();
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

        public virtual ICollection<CommentLike> CommentLikes
        {
            get { return this.commentLikes; }
            set { this.commentLikes = value; }
        }

    }
}