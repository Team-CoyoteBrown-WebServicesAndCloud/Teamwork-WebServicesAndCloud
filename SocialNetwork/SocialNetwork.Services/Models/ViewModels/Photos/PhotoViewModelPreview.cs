namespace SocialNetwork.Services.Models.ViewModels.Photos
{
    using System;
    using System.Linq.Expressions;
    using SocialNetwork.Models;

    public class PhotoViewModelPreview : BasePhotoViewModel
    {
        public static Expression<Func<Photo, PhotoViewModelPreview>> Create
        {
            get
            {
                return photo => new PhotoViewModelPreview
                {
                    Id = photo.Id,
                    Image = photo.Source
                };
            }
        } 
    }
}