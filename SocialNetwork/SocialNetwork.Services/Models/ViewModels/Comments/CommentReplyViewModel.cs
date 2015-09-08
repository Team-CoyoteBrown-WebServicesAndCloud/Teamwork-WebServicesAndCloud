namespace SocialNetwork.Services.Models.ViewModels.Comments
{
    using System.Linq;
    using SocialNetwork.Models;
    using User;

    public class CommentReplyViewModel : BaseCommentViewModel
    {
        public static CommentReplyViewModel Create(CommentReply comment, ApplicationUser currentUser)
        {
            return new CommentReplyViewModel
            {
                Id = comment.Id,
                Author = UserViewModelMinified.ConvertTo(comment.Author),
                Date = comment.RepliedOn,
                CommentContent = comment.Content,
                LikesCount = comment.Likes.Count,
                Liked = comment.Likes.Any(l => l.UserId == currentUser.Id)
            };
        }
    }
}