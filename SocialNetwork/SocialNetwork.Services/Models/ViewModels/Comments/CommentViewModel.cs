namespace SocialNetwork.Services.Models.ViewModels.Comments
{
    using System.Collections.Generic;
    using System.Linq;
    using SocialNetwork.Models;
    using User;

    public class CommentViewModel : BaseCommentViewModel
    {
        public IEnumerable<CommentReplyViewModel> CommentReplies { get; set; }

        public static CommentViewModel Create(Comment comment, ApplicationUser currentUser)
        {
            return new CommentViewModel()
            {
                Id = comment.Id,
                Author = UserViewModelMinified.ConvertTo(comment.Author),
                Date = comment.PostedOn,
                CommentContent = comment.Content,
                LikesCount = comment.Likes.Count,
                Liked = comment.Likes.Any(l => l.UserId == currentUser.Id),
                CommentReplies = comment.Replies
                    .OrderByDescending(cr => cr.RepliedOn)
                    .Select(reply => new CommentReplyViewModel
                    {
                        Id = reply.Id,
                        Author = UserViewModelMinified.ConvertTo(reply.Author),
                        Date = reply.RepliedOn,
                        CommentContent = reply.Content,
                        LikesCount = reply.Likes.Count,
                        Liked = reply.Likes.Any(l => l.UserId == currentUser.Id),
                    })
            };
        }
    }
}