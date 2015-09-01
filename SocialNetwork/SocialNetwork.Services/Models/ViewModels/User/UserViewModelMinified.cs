﻿namespace SocialNetwork.Services.Models.ViewModels.User
{
    using System;
    using System.Linq.Expressions;
    using SocialNetwork.Models;

    public class UserViewModelMinified : BaseUserViewModel
    {
        public static UserViewModelMinified ConvertTo(ApplicationUser user)
        {
            UserViewModelMinified userViewModel = new UserViewModelMinified
                {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.UserName,
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