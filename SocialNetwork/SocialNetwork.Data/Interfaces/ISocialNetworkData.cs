namespace SocialNetwork.Data.Interfaces
{
    using Models;

    public interface ISocialNetworkData
    {
        IRepository<ApplicationUser> Users { get; }

        IRepository<Comment> Comments { get; }

        IRepository<CommentLike> CommentLikes { get; }

        IRepository<CommentReply> CommentReplies { get; }

        IRepository<FriendRequest> FriendRequests { get; }

        IRepository<Group> Groups { get; }

        IRepository<GroupPost> GroupPosts { get; }
        
        IRepository<Notification> Notifications { get; }

        IRepository<Photo> Photos { get; }

        IRepository<PhotoLike> PhotoLikes { get; }

        IRepository<Post> Posts { get; }

        IRepository<PostLike> PostLikes { get; }

        IRepository<UserSession> UserSessions { get; }

        int SaveChanges();
    }
}
