namespace SocialNetwork.Services.Models.ViewModels.User
{
    using System;
    using System.Linq.Expressions;
    using SocialNetwork.Models;

    public class CurrentUserViewModel : BaseUserViewModel
    {
        public string CoverImageData { get; set; }

        public static Expression<Func<ApplicationUser, CurrentUserViewModel>> Create
        {
            get
            {
                return user =>  new CurrentUserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.UserName,
                    Gender = user.Gender,
                    ProfileImageData = user.ProfileImageData,
                    CoverImageData = user.CoverImageData,
                };
            }
        }
    }
}