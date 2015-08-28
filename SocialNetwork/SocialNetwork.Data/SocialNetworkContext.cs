namespace SocialNetwork.Data
{
 
    using Microsoft.AspNet.Identity.EntityFramework;
    using SocialNetwork.Models;
    using System.Data.Entity;

    public class SocialNetworkContext : IdentityDbContext
    {
    
        public SocialNetworkContext()
            : base("name=SocialNetworkContext")
        {
        }

        public virtual IDbSet<Post> Posts { get; set; }

        public virtual IDbSet<Comment> Comments { get; set; }

        public IDbSet<FriendRequest> FriendRequests { get; set; }

        public IDbSet<PostLike> PostLikes { get; set; }

        public IDbSet<CommentLike> CommentLikes { get; set; }


        public static SocialNetworkContext Create()
        {
            return new SocialNetworkContext();
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Post>()
               .HasRequired<ApplicationUser>(p => p.Author)
               .WithMany(a => a.OwnPosts)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
                .HasRequired<ApplicationUser>(p => p.WallOwner)
                .WithMany(o => o.WallPosts)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Comment>()
                .HasRequired<ApplicationUser>(p => p.Author)
                .WithMany(a => a.Comments)
                .WillCascadeOnDelete(false);

        }

    }
}