namespace SocialNetwork.Services.Models.ViewModels.Photos
{
    using System;
    using System.Linq.Expressions;
    using SocialNetwork.Models;

    public class PhotoViewModelMinified : BasePhotoViewModel
    {
        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public static Expression<Func<Photo, PhotoViewModelMinified>> Create
        {
            get
            {
                return photo => new PhotoViewModelMinified
                {
                    Id = photo.Id,
                    Image = photo.Source,
                    LikesCount = photo.Likes.Count,
                    CommentsCount = photo.Comments.Count
                };
            }
        } 

    }
}