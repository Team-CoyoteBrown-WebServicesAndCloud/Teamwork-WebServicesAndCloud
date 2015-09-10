namespace SocialNetwork.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Enum;

    public class Group
    {
        private ICollection<ApplicationUser> members;
        private ICollection<ApplicationUser> admins;
        private ICollection<GroupPost> posts;

        public Group()
        {
            this.members = new HashSet<ApplicationUser>();
            this.admins = new HashSet<ApplicationUser>();
            this.posts = new HashSet<GroupPost>();
        }
       
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string CoverImageData { get; set; }

        [Required]
        public GroupType Type { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public virtual ICollection<ApplicationUser> Admins
        {
            get { return this.admins; }
            set { this.admins = value; }
        }

        public virtual ICollection<ApplicationUser> Members
        {
            get { return this.members; }
            set { this.members = value; }
        }

        public virtual ICollection<GroupPost> Posts
        {
            get { return this.posts; }
            set { this.posts = value; }
        }
    }
}