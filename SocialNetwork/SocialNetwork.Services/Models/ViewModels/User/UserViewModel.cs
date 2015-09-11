namespace SocialNetwork.Services.Models.ViewModels.User
{
    using System.Linq;
    using Groups;
    using SocialNetwork.Models;
    using SocialNetwork.Models.Enum;

    public class UserViewModel : BaseUserViewModel
    {
        public string CoverImageData { get; set; }

        public bool IsFriend { get; set; }

        public bool HasPendingRequest { get; set; }

        public IQueryable<GroupViewModelPreview> Groups { get; set; } 

        public static UserViewModel ConvertTo(ApplicationUser user, ApplicationUser currentUser)
        {
            UserViewModel wantedUser = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.UserName,
                Gender = user.Gender,
                ProfileImageData = user.ProfileImageData,
                CoverImageData = user.CoverImageData,
                IsFriend = user.Friends.Any(f => f.Id == currentUser.Id),
                HasPendingRequest = user.FriendRequests.Any(
                    r => r.FriendRequestStatus == FriendRequestStatus.AwaitingApproval &&
                    (r.FromUserId == currentUser.Id)),
                Groups = user.Groups
                    .AsQueryable()
                    .Select(GroupViewModelPreview.Create)
            };

            return wantedUser;
        }
    }
}