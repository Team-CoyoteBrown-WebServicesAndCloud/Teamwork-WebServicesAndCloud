namespace SocialNetwork.Services.Models.ViewModels.User
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using SocialNetwork.Models;

    public class UserViewModelMinified : BaseUserViewModel
    {
        public bool IsFriend { get; set; }

        public static Expression<Func<ApplicationUser, UserViewModelMinified>> Create(ApplicationUser user)
        {
            return userViewModel => new UserViewModelMinified
            {
                Id = userViewModel.Id,
                Name = userViewModel.Name,
                Username = userViewModel.UserName,
                IsFriend = userViewModel.Friends.Any(f => f.Id == user.Id),
                Gender = userViewModel.Gender,
                ProfileImageData = userViewModel.ProfileImageData
            };
        }
    }
}