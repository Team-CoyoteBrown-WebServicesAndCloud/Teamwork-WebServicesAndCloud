namespace SocialNetwork.Data.Data
{
    using System.Data.Entity;
    using Interfaces;
    using Microsoft.AspNet.Identity.EntityFramework;
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

        public IDbSet<PhotoLike> PhotosLikes { get; set; }

        public IDbSet<Post> Posts { get; set; }

        public IDbSet<Comment> Comments { get; set; }

        public IDbSet<CommentReply> CommentReplies { get; set; }

        public IDbSet<FriendRequest> FriendRequests { get; set; }

        public IDbSet<Group> Groups { get; set; }

        public IDbSet<GroupPost> GroupPosts { get; set; }

        public IDbSet<Notification> Notifications { get; set; }

        public IDbSet<Photo> Photos { get; set; }

        public IDbSet<PostLike> PostLikes { get; set; }

        public IDbSet<CommentLike> CommentLikes { get; set; }

        public IDbSet<UserSession> Sessions { get; set; }

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

            modelBuilder.Entity<FriendRequest>()
                .HasRequired<ApplicationUser>(r => r.FromUser)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FriendRequest>()
                .HasRequired<ApplicationUser>(r => r.ToUser)
                .WithMany(u => u.FriendRequests)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Photo>()
                .HasRequired<ApplicationUser>(p => p.PhotoOwner)
                .WithMany(u => u.Photos)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PhotoLike>()
                .HasRequired<ApplicationUser>(p => p.PhotoOwner)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CommentReply>()
                .HasRequired<Comment>(c => c.Comment)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Notification>()
                .HasRequired<ApplicationUser>(n => n.Receiver)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany<ApplicationUser>(u => u.Friends)
                .WithMany()
                .Map(uu =>
                {
                    uu.MapLeftKey("UserId");
                    uu.MapRightKey("FriendId");
                    uu.ToTable("UserFriends");
                });

            modelBuilder.Entity<Group>()
                .HasMany<ApplicationUser>(g => g.Users)
                .WithMany()
                .Map(gu =>
                {
                    gu.MapLeftKey("GroupId");
                    gu.MapRightKey("UserId");
                    gu.ToTable("UserGroups");
                });

            base.OnModelCreating(modelBuilder);
        }

    }
}