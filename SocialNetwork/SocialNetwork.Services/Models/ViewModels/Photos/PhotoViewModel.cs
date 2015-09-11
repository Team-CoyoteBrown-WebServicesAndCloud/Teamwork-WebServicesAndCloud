namespace SocialNetwork.Services.Models.ViewModels.Photos
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Comments;
    using SocialNetwork.Models;
    using User;

    public class PhotoViewModel : BasePhotoViewModel
    {
        public int LikesCount { get; set; }

        public bool Liked { get; set; }

        public int CommentsCount { get; set; }

        public string Description { get; set; }

        public DateTime PostedOn { get; set; }

        public IQueryable<CommentViewModel> Comments { get; set; }

        public UserViewModelMinified Owner { get; set; }

        public static Expression<Func<Photo, PhotoViewModel>> Create(ApplicationUser currentUser)
        {
            string currentUserId = currentUser.Id;
            return photo => new PhotoViewModel
            {
                Id = photo.Id,
                Image = photo.Source,
                Description = photo.Description,
                PostedOn = photo.PostedOn,
                LikesCount = photo.Likes.Count,
                Liked = photo.Likes.Any(l => l.UserId == currentUserId),
                CommentsCount = photo.Comments.Count,
                Owner = new UserViewModelMinified
                {
                    Id = photo.PhotoOwner.Id,
                    Name = photo.PhotoOwner.Name,
                    Username = photo.PhotoOwner.UserName,
                    IsFriend = photo.PhotoOwner.Friends.Any(f => f.Id == currentUserId),
                    Gender = photo.PhotoOwner.Gender,
                    ProfileImageData = photo.PhotoOwner.ProfileImageData,
                },
                Comments = photo.Comments
                    .AsQueryable()
                    .Select(CommentViewModel.Create(currentUser))
            };
        }
    }
}