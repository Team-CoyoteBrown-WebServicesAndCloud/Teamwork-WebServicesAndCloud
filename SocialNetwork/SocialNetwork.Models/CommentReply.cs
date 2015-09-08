namespace SocialNetwork.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

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

        [Required]
        public DateTime RepliedOn { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }

        public virtual ICollection<CommentLike> Likes
        {
            get { return this.likes; }
            set { this.likes = value; }
        }
    }
}