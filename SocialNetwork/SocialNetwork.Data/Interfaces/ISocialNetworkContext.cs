namespace SocialNetwork.Data.Interfaces
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using Models;

    public interface ISocialNetworkContext : IDisposable
    {
        IDbSet<Post> Posts { get; set; }

        IDbSet<Comment> Comments { get; set; }

        IDbSet<FriendRequest> FriendRequests { get; set; }

        IDbSet<PostLike> PostLikes { get; set; }

        IDbSet<CommentLike> CommentLikes { get; set; }
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges();
    }
}
