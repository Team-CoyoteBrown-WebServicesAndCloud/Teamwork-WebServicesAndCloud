namespace SocialNetwork.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        private ICollection<CommentLike> likes;
        private ICollection<CommentReply> replies;

        public Comment()
        {
            this.likes = new HashSet<CommentLike>();
            this.replies = new HashSet<CommentReply>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        public string Content { get; set; }

        [Required]
        public DateTime PostedOn { get; set; }

        [Required]
        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public ICollection<CommentLike> Likes
        {
            get { return this.likes; }
            set { this.likes = value; }
        } 

        public ICollection<CommentReply> Replies
        {
            get { return this.replies; }
            set { this.replies = value; }
        } 
    }
}