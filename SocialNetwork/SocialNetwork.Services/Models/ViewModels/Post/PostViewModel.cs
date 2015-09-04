namespace SocialNetwork.Services.Models.ViewModels.Post
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Comments;
    using SocialNetwork.Models;
    using User;

    public class PostViewModel
    {
        public int Id { get; set; }

        public UserViewModelMinified Author { get; set; }

        public UserViewModelMinified WallOwner { get; set; }

        public string PostContent { get; set; }

        public DateTime Date { get; set; }

        public int LikesCount { get; set; }

        public bool Liked { get; set; }

        public int TotalCommentsCount { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public static PostViewModel Create(Post post, ApplicationUser currentUser)
        {
            return new PostViewModel()
            {
                Id = post.Id,
                Author = UserViewModelMinified.ConvertTo(post.Author),
                WallOwner = UserViewModelMinified.ConvertTo(post.WallOwner),
                PostContent = post.Content,
                Date = post.PostedOn,
                LikesCount = post.Likes.Count,
                Liked = post.Likes.Any(l => l.UserId == currentUser.Id),
                TotalCommentsCount = post.Comments.Count,
                Comments = post.Comments
                    .Reverse()
                    .Take(4)
                    .Select(c => CommentViewModel.Create(c, currentUser))
            };
        }
    }
}