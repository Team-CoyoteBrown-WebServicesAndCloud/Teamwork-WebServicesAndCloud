namespace SocialNetwork.Services.Models.ViewModels.User
{
    using SocialNetwork.Models;
    using SocialNetwork.Models.Enum;

    public class FriendRequestsViewModel
    {
        public int Id { get; set; }

        public FriendRequestStatus Status { get; set; }

        public UserViewModelMinified User { get; set; }

        public static FriendRequestsViewModel Create(FriendRequest request)
        {
            return new FriendRequestsViewModel()
            {
                Id = request.Id,
                Status = request.FriendRequestStatus,
                User = UserViewModelMinified.ConvertTo(request.FromUser)
            };
        }
    }
}