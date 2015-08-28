namespace SocialNetwork.Data
{
 
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;
    using Interfaces;
    using Migrations;
    using Models;

    public class SocialNetworkContext : IdentityDbContext , ISocialNetworkContext
    {
    
        public SocialNetworkContext()
            : base("name=SocialNetworkContext")
        {
            var migration = new MigrateDatabaseToLatestVersion<SocialNetworkContext,Configuration>();
            Database.SetInitializer(migration);
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
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public new int SaveChanges()
        {
            return base.SaveChanges();
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

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        }

    }
}