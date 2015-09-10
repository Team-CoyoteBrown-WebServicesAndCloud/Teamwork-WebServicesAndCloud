namespace SocialNetwork.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Enum;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationUser : IdentityUser
    {
        private ICollection<Comment> comments;
        private ICollection<FriendRequest> friendRequests;
        private ICollection<ApplicationUser> friends;
        private ICollection<Group> groups;
        private ICollection<Post> ownPosts;
        private ICollection<Photo> photos;
        private ICollection<Post> wallPosts;

        public ApplicationUser()
        {
            this.ownPosts = new HashSet<Post>();
            this.wallPosts = new HashSet<Post>();
            this.comments = new HashSet<Comment>();
            this.friends = new HashSet<ApplicationUser>();
            this.friendRequests = new HashSet<FriendRequest>();
            this.photos = new HashSet<Photo>();
            this.groups = new HashSet<Group>();
        }

        [MinLength(2)]
        [MaxLength(30)]
        public string Name { get; set; }

        public string ProfileImageData { get; set; }

        public string CoverImageData { get; set; }

        public string ProfileImageDataMinified { get; set; }

        public string Location { get; set; }

        [Range(0, 100)]
        public int? Age { get; set; }

        public Gender? Gender { get; set; }

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

        public virtual ICollection<FriendRequest> FriendRequests
        {
            get { return this.friendRequests; }
            set { this.friendRequests = value; }
        }

        public virtual ICollection<Photo> Photos
        {
            get { return this.photos; }
            set { this.photos = value; }
        }

        public virtual ICollection<Group> Groups
        {
            get { return this.groups; }
            set { this.groups = value; }
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