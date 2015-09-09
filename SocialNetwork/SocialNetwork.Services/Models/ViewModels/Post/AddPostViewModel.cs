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

        public static AddPostViewModel ConvertTo(Post post, ApplicationUser currentUser)
        {
            AddPostViewModel postViewModel = new AddPostViewModel
            {
                Id = post.Id,
                Author = new UserViewModelMinified
                {
                    Id = post.AuthorId,
                    Name = post.Author.Name,
                    Username = post.Author.UserName,
                    IsFriend = post.Author.Friends.Any(f => f.Id == currentUser.Id),
                    Gender = post.Author.Gender,
                    ProfileImageData = post.Author.ProfileImageData,
                },
                WallOwner = new UserViewModelMinified
                {
                    Id = post.WallOwnerId,
                    Name = post.WallOwner.Name,
                    Username = post.WallOwner.UserName,
                    IsFriend = post.WallOwner.Friends.Any(f => f.Id == currentUser.Id),
                    Gender = post.WallOwner.Gender,
                    ProfileImageData = post.WallOwner.ProfileImageData,
                },
                PostContent = post.Content,
                PostedOn = post.PostedOn,
                LikesCount = post.Likes.Count,
                Liked = post.Likes.Any(l => l.UserId == currentUser.Id),
                TotalCommentsCount = post.Comments.Count,
                Comments = post.Comments
                    .Reverse()
                    .Take(4)
                    .AsQueryable()
                    .Select(CommentViewModel.Create(currentUser))
            };

            return postViewModel;
        }
    }
}