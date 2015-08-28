using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SocialNetwork.Models
{
    public class Group
    {
        private ICollection<ApplicationUser> groupUsers;
        private ICollection<GroupPost> groupPosts;

        public Group()
        {
            this.groupUsers = new HashSet<ApplicationUser>();
            this.groupPosts = new HashSet<GroupPost>();
        }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string CoverImageData { get; set; }

        public virtual ICollection<ApplicationUser> GroupUsers
        {
            get { return this.groupUsers; }
            set { this.groupUsers = value; }
        }

        public virtual ICollection<GroupPost> GroupPosts
        {
            get { return this.groupPosts; }
            set { this.groupPosts = value; }
        }



    }
}