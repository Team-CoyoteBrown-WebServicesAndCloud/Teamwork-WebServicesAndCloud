namespace SocialNetwork.Services.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Data.Data;
    using Data.Interfaces;
    using Infrastructure;
    using Models.BindingModels;
    using Models.ViewModels.User;
    using SocialNetwork.Models.Enum;
    using UserSessionUtils;

    [SessionAuthorize]
    [RoutePrefix("api/me")]
    public class ProfileController : BaseApiController
    {
        public ProfileController()
            : this(new SocialNetworkContextData(), new AspNetUserIdProvider())
        {
        }

        public ProfileController(ISocialNetworkData data, IUserIdProvider userIdProvider)
            : base(data, userIdProvider)
        {
        }

        [HttpGet]
        [Route]
        public IHttpActionResult GetCurrentUserData()
        {
            var userId = this.UserIdProvider.GetUserId();
            var user = this.Data
                .Users
                .All()
                .Where(u => u.Id == userId)
                .Select(CurrentUserViewModel.Create);
            
            return this.Ok(user);
        }

        [HttpPut]
        public IHttpActionResult EditUserProfil(EditProfilBindingModel model)
        {
            var userId = this.UserIdProvider.GetUserId();
            var user = this.Data.Users.Find(userId);

            user.Name = model.Name;
            user.Email = model.Email;
            user.Gender = model.Gender;

            this.Data.Users.Update(user);
            this.Data.SaveChanges();

            return this.Ok();
        }

        [HttpGet]
        [Route("friends")]
        public IHttpActionResult GetFriendsData()
        {
            var userId = this.UserIdProvider.GetUserId();
            if (userId == null)
            {
                return this.BadRequest();
            }

            var user = this.Data.Users.Find(userId);
            var friends = user.Friends
                .AsQueryable()
                .Select(UserViewModelMinified.Create);

            return this.Ok(friends);
        }

        [HttpGet]
        [Route("friends/preview")]
        public IHttpActionResult GetFriendsPreviewData()
        {
            var currentUserId = this.UserIdProvider.GetUserId();

            var user = this.Data.Users.Find(currentUserId);
            var friends = user.Friends
                .AsQueryable()
                .Take(6)
                .Select(UserViewModelMinified.Create);

            return this.Ok(new
            {
                totalCount = user.Friends.Count,
                friends
            });
        }

        [HttpGet]
        [Route("requests")]
        public IHttpActionResult GetFriendRequests()
        {
            var currentUserId = this.UserIdProvider.GetUserId();

            var user = this.Data.Users.Find(currentUserId);
            var requests = user.FriendRequests
                .Where(r => r.FriendRequestStatus == FriendRequestStatus.AwaitingApproval)
                .Select(FriendRequestsViewModel.Create);

            return this.Ok(requests);
        }

        [HttpGet]
        [Route("feed")]
        public IHttpActionResult GetNewsFeed(int postsCount, int startPostNumber)
        {

            var currentUserId = this.UserIdProvider.GetUserId();

            var user = this.Data.Users.Find(currentUserId);
            var posts = user.Friends.Select(f => f.OwnPosts).Skip(startPostNumber).Take(postsCount);

            return this.Ok(posts.Count());
        }

        [HttpPut]
        [Route("requests/approve")]
        public IHttpActionResult ApproveFriendRequest(int id)
        {
            var currentUserId = this.UserIdProvider.GetUserId();

            var user = this.Data.Users.Find(currentUserId);

            var friendId = user.FriendRequests
                .Where(c => c.Id == id)
                .Select(c => c.FromUserId)
                .FirstOrDefault();
            var friend = this.Data.Users.Find(friendId);

            var request = user.FriendRequests.FirstOrDefault(c => c.Id == id);
            request.FriendRequestStatus = FriendRequestStatus.Approved;

            user.Friends.Add(friend);
            this.Data.SaveChanges();
            return this.Ok();

        }
        [HttpPut]
        [Route("requests/reject")]
        public IHttpActionResult RejectFriendRequest(int id)
        {
            var currentUserId = this.UserIdProvider.GetUserId();

            var user = this.Data.Users.Find(currentUserId);

            var request = user.FriendRequests.FirstOrDefault(c => c.Id == id);
            request.FriendRequestStatus = FriendRequestStatus.Declined;

            this.Data.Users.Update(user);
            this.Data.SaveChanges();
            return this.Ok();

        }

        [HttpGet]
        [Route("search")]
        public IHttpActionResult SearchUsers(string searchTerm)
        {
            var users = this.Data.Users.All().Where(c => c.Name.Contains(searchTerm));
            return this.Ok(users);
        }
    }
}
