namespace SocialNetwork.Services.Models.ViewModels
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using SocialNetwork.Models;
    using SocialNetwork.Models.Enum;
    using User;

    public class FriendRequestViewModel
    {
        public int Id { get; set; }

        public FriendRequestStatus Status { get; set; }

        public UserViewModelMinified User { get; set; }

        public static Expression<Func<FriendRequest, FriendRequestViewModel>> Create(ApplicationUser currentUser)
        {
            return request =>  new FriendRequestViewModel()
            {
                Id = request.Id,
                Status = request.FriendRequestStatus,
                User = new UserViewModelMinified
                {
                    Id = request.FromUserId,
                    Name = request.FromUser.Name,
                    Username = request.FromUser.UserName,
                    IsFriend = request.FromUser.Friends.Any(f => f.Id == currentUser.Id),
                    Gender = request.FromUser.Gender,
                    ProfileImageData = request.FromUser.ProfileImageData
                }
            };
        }
    }
}