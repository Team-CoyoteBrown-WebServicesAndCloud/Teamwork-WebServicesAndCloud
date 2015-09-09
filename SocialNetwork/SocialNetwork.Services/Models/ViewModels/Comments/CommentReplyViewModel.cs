namespace SocialNetwork.Services.Models.ViewModels.Comments
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using SocialNetwork.Models;
    using User;

    public class CommentReplyViewModel : BaseCommentViewModel
    {
        public static Expression<Func<CommentReply, CommentReplyViewModel>> Create(ApplicationUser currentUser)
        {
            string currentUserId = currentUser.Id;
            return comment => new CommentReplyViewModel
            {
                Id = comment.Id,
                Author = new UserViewModelMinified
                {
                    Id = comment.Author.Id,
                    Name = comment.Author.Name,
                    Username = comment.Author.UserName,
                    IsFriend = comment.Author.Friends.Any(f => f.Id == currentUserId),
                    Gender = comment.Author.Gender,
                    ProfileImageData = comment.Author.ProfileImageData,
                },
                Date = comment.RepliedOn,
                CommentContent = comment.Content,
                LikesCount = comment.Likes.Count,
                Liked = comment.Likes.Any(l => l.UserId == currentUserId)
            };
        }
    }
}