using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SocialNetwork.Models
{
    public class Photo
    {
        private ICollection<Comment> comments;
        private ICollection<PhotoLike> likes;

        public Photo()
        {
            this.comments = new HashSet<Comment>();
            this.likes = new HashSet<PhotoLike>();
        }
        public int Id { get; set; }

        [Required]
        public string Source { get; set; }

        //content
        public string Description { get; set; }

        public DateTime UploadedOn { get; set; }

        [Required]
        public int PhotoOwnerId { get; set; }

        public virtual ApplicationUser PhotoOwner { get; set; }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public virtual ICollection<PhotoLike> Likes
        {
            get { return this.likes; }
            set { this.likes = value; }
        }
    }
}