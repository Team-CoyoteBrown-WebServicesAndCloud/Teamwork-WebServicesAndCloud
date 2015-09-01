namespace SocialNetwork.Services.Models.ViewModels.User
{
    using System.Linq;
    using SocialNetwork.Models;
    using SocialNetwork.Models.Enum;

    public class UserViewModel : BaseUserViewModel
    {
        public string CoverImageData { get; set; }

        public bool IsFriend { get; set; }

        public bool HasPendingRequest { get; set; }

        public static UserViewModel Create(ApplicationUser user, ApplicationUser currentLoggedUser)
        {
            UserViewModel userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.UserName,
                    Gender = user.Gender,
                    ProfileImageData = user.ProfileImageData,
                    CoverImageData = user.CoverImageData,
                    IsFriend = user.Friends.Contains(currentLoggedUser),
                    HasPendingRequest = user.FriendRequests.Any(
                        r => r.FriendRequestStatus == FriendRequestStatus.AwaitingApproval &&
                        (r.FromUserId == currentLoggedUser.Id))
                };

            return userViewModel;
        }
    }
}