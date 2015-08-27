namespace SocialNetwork.Models
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SocialNetwork.Models.Enum;
    using System.ComponentModel.DataAnnotations;

    public class ApplicationUser : IdentityUser, IUser
    {
        private ICollection<Post> ownPosts;
        private ICollection<Post> wallPosts;
        private ICollection<Comment> comments;
        private ICollection<ApplicationUser> friends;
        private ICollection<FriendRequest> friendRequests;

        public ApplicationUser()
        {
            this.ownPosts = new HashSet<Post>();
            this.wallPosts = new HashSet<Post>();
            this.comments = new HashSet<Comment>();
            this.friends = new HashSet<ApplicationUser>();
            this.friendRequests = new HashSet<FriendRequest>();

        }

        [Required]
        public string Name { get; set; }

        public string ProfileImageData { get; set; }

        public string CoverImageData { get; set; }
        
        public string ProfileImageDataMinified { get; set; }

        public string Location { get; set; }

        public int? Age { get; set; }

        public Gender Gender { get; set; }

        public virtual ICollection<Post> OwnPosts
        {
            get { return this.ownPosts; }
            set { this.ownPosts = value; }
        }

        public virtual ICollection<Post> WallPosts
        {
            get { return this.wallPosts; }
            set { this.wallPosts = value; }
        }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public virtual ICollection<ApplicationUser> Friends
        {
            get { return this.friends; }
            set { this.friends = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<ApplicationUser> manager,
            string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            return userIdentity;
        }
    }
}