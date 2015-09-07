namespace SocialNetwork.Services.Models.ViewModels.User
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using SocialNetwork.Models;

    public class UserViewModelMinified : BaseUserViewModel
    {
        public bool IsFriend { get; set; }

        public static UserViewModelMinified ConvertTo(ApplicationUser user)
        {
            UserViewModelMinified userViewModel = new UserViewModelMinified
                {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.UserName,
                    IsFriend = user.Friends.Any(f => f.Id == user.Id),
                    Gender = user.Gender,
                    ProfileImageData = user.ProfileImageData,
                };

            return userViewModel;
        }

        public static Expression<Func<ApplicationUser, UserViewModelMinified>> Create
        {
            get
            {
                return user => new UserViewModelMinified
                {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.UserName,
                    Gender = user.Gender,
                    ProfileImageData = user.ProfileImageData,
                };
            }
        }
    }
}