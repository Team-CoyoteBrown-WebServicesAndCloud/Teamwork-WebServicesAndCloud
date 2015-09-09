namespace SocialNetwork.Services.Models.ViewModels.Comments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using SocialNetwork.Models;
    using User;

    public class CommentViewModel : BaseCommentViewModel
    {
        public IQueryable<CommentReplyViewModel> CommentReplies { get; set; }

        public static Expression<Func<Comment, CommentViewModel>> Create(ApplicationUser currentUser)
        {
            string currentUserId = currentUser.Id;
            return comment => new CommentViewModel
            {
                Id = comment.Id,
                Author = new UserViewModelMinified
                {
                    Id = comment.AuthorId,
                    Name = comment.Author.Name,
                    Username = comment.Author.UserName,
                    IsFriend = comment.Author.Friends.Any(f => f.Id == currentUserId),
                    Gender = comment.Author.Gender,
                    ProfileImageData = comment.Author.ProfileImageData,
                },
                Date = comment.PostedOn,
                CommentContent = comment.Content,
                LikesCount = comment.Likes.Count,
                Liked = comment.Likes.Any(l => l.UserId == currentUserId),
                CommentReplies = comment.Replies
                    .AsQueryable()
                    .OrderByDescending(cr => cr.RepliedOn)
                    .Select(CommentReplyViewModel.Create(currentUser))
            };
        }

        public static CommentViewModel ConvertTo(Comment comment, ApplicationUser currentUser)
        {
            CommentViewModel commentViewModel = new CommentViewModel
            {
                Id = comment.Id,
                Author = new UserViewModelMinified
                {
                    Id = comment.AuthorId,
                    Name = comment.Author.Name,
                    Username = comment.Author.UserName,
                    IsFriend = comment.Author.Friends.Any(f => f.Id == currentUser.Id),
                    Gender = comment.Author.Gender,
                    ProfileImageData = comment.Author.ProfileImageData,
                },
                Date = comment.PostedOn,
                CommentContent = comment.Content,
                LikesCount = comment.Likes.Count,
                Liked = comment.Likes.Any(l => l.UserId == currentUser.Id),
                CommentReplies = comment.Replies
                    .OrderByDescending(cr => cr.RepliedOn)
                    .AsQueryable()
                    .Select(CommentReplyViewModel.Create(currentUser))
            };

            return commentViewModel;
        }
    }
}