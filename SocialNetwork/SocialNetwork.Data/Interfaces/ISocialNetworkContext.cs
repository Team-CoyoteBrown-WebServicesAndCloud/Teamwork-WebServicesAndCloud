namespace SocialNetwork.Data.Interfaces
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using Models;

    public interface ISocialNetworkContext : IDisposable
    {
        IDbSet<Comment> Comments { get; set; }

        IDbSet<CommentLike> CommentLikes { get; set; }

        IDbSet<CommentReply> CommentReplies { get; set; }

        IDbSet<FriendRequest> FriendRequests { get; set; }

        IDbSet<Group> Groups { get; set; }
        
        IDbSet<GroupPost> GroupPosts { get; set; }

        IDbSet<Notification> Notifications { get; set; }
        
        IDbSet<Photo> Photos { get; set; }

        IDbSet<PhotoLike> PhotosLikes { get; set; }

        IDbSet<Post> Posts { get; set; }

        IDbSet<PostLike> PostLikes { get; set; }

        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges();
    }
}
