namespace SocialNetwork.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Group
    {
        private ICollection<ApplicationUser> users;
        private ICollection<GroupPost> posts;

        public Group()
        {
            this.users = new HashSet<ApplicationUser>();
            this.posts = new HashSet<GroupPost>();
        }
       
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string CoverImageData { get; set; }

        public virtual ICollection<ApplicationUser> Users
        {
            get { return this.users; }
            set { this.users = value; }
        }

        public virtual ICollection<GroupPost> Posts
        {
            get { return this.posts; }
            set { this.posts = value; }
        }
    }
}