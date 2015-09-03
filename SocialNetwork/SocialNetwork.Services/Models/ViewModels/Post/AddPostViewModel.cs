namespace SocialNetwork.Services.Models.ViewModels.Post
{
    using System.Collections.Generic;
    using System.Linq;
    using Comments;
    using SocialNetwork.Models;
    using User;

    public class AddPostViewModel : BasePostViewModel
    {
        public UserViewModelMinified Author { get; set; }

        public UserViewModelMinified WallOwner { get; set; }

        public int LikesCount { get; set; }

        public bool Liked { get; set; }

        public int TotalCommentsCount { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public static AddPostViewModel Create(Post post, ApplicationUser currentUser)
        {
            return new AddPostViewModel
            {
                Id = post.Id,
                Author = UserViewModelMinified.ConvertTo(post.Author),
                WallOwner = UserViewModelMinified.ConvertTo(post.WallOwner),
                Content = post.Content,
                PostedOn = post.PostedOn,
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