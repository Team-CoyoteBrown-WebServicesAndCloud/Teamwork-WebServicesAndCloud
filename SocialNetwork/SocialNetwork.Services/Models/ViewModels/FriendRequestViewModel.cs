namespace SocialNetwork.Services.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;
    using SocialNetwork.Models;
    using SocialNetwork.Models.Enum;
    using User;

    public class FriendRequestViewModel
    {
        public int Id { get; set; }

        public FriendRequestStatus Status { get; set; }

        public UserViewModelMinified User { get; set; }

        public static Expression<Func<FriendRequest, FriendRequestViewModel>> Create
        {
            get
            {
                return request =>  new FriendRequestViewModel()
                {
                    Id = request.Id,
                    Status = request.FriendRequestStatus,
                    User = UserViewModelMinified.ConvertTo(request.FromUser)
                };
            }
        }
    }
}