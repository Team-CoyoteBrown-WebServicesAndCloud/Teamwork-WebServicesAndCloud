namespace SocialNetwork.Data.Interfaces
{
    using Models;

    public interface ISocialNetworkData
    {
        IRepository<ApplicationUser> Users { get; }
        IRepository<Post> Posts { get; }
        IRepository<Comment> Comments { get; }

        IRepository<FriendRequest> FriendRequests { get; }

        IRepository<PostLike> PostLikes { get; }

        IRepository<CommentLike> CommentLikes { get; }
        int SaveChanges();
    }
}
