namespace SocialNetwork.Services.Controllers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Http;
    using Data.Data;
    using Data.Interfaces;
    using Infrastructure;
    using Models.BindingModels;
    using Models.ViewModels;
    using Models.ViewModels.Comments;
    using Models.ViewModels.Post;
    using Models.ViewModels.User;
    using SocialNetwork.Models;
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
        [Route("friends/{username}/preview")]
        public IHttpActionResult GetFriendsPreviewData(string username)
        {
            var currentUserId = this.UserIdProvider.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);

            var existingUser = this.Data.Users.All().FirstOrDefault(c => c.UserName == username);
            if (existingUser == null)
            {
                return this.NotFound();
            }

            string userStatus;
            if (currentUser.Friends.Contains(existingUser))
            {
                userStatus = "friend";
            }
            else
            {
                userStatus = "invite";
            }

            if (existingUser.FriendRequests.Any(fr => fr.FromUserId == currentUserId) && (!currentUser.Friends.Contains(existingUser)))
            {
                userStatus = "pending";
            }

            return this.Ok(new
            {
                Id = existingUser.Id,
                Name = existingUser.Name,
                Username = existingUser.UserName,
                Gender = existingUser.Gender,
                ProfileImageData = existingUser.ProfileImageData,
                UserStatus = userStatus
            });
        }

        [HttpGet]
        [Route("requests")]
        public IHttpActionResult GetFriendRequests()
        {
            var currentUserId = this.UserIdProvider.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);
            var requests = currentUser.FriendRequests
                .AsQueryable()
                .Where(r => r.FriendRequestStatus == FriendRequestStatus.AwaitingApproval)
                .Select(FriendRequestViewModel.Create);

            return this.Ok(requests);
        }

        [HttpPost]
        [Route("requests/{username}")]

        public IHttpActionResult SendFriendRequests(string username)
        {
            var currentUserId = this.UserIdProvider.GetUserId();
            var existingUser = this.Data
                .Users
                .All()
                .FirstOrDefault(c => c.Name == username);
            if (existingUser == null)
            {
                return this.NotFound();
            }

            FriendRequest friendRequest = new FriendRequest
            {
                FriendRequestStatus = FriendRequestStatus.AwaitingApproval,
                FromUserId = currentUserId,
                ToUserId = existingUser.Id,
            };

            existingUser.FriendRequests.Add(friendRequest);
            this.Data.SaveChanges();

            return this.Ok();
        }

        [HttpGet]
        [Route("feed")]
        public IHttpActionResult GetNewsFeed([FromUri]NewsFeedBindingModel bindingModel)
        {
            var currentUserId = this.UserIdProvider.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);

            PostViewModel.CurrentUser = currentUser;
            var posts = this.Data
                .Posts
                .All()
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Where(p => p.Author.Friends.Any(f => f.Id == currentUserId) ||
                    p.WallOwner.Friends.Any(f => f.Id == currentUserId))
                .OrderByDescending(p => p.PostedOn)
                .Skip(bindingModel.StartPostNumber)
                .Take(bindingModel.PostsCount)
                .Select(PostViewModel.Create);

            return this.Ok(posts);
        }

        [HttpPut]
        [Route("requests/{requestId}/approve")]
        public IHttpActionResult ApproveFriendRequest(int requestId)
        {
            var currentUserId = this.UserIdProvider.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);

            var existingUser = currentUser.FriendRequests
                .Where(r => r.Id == requestId)
                .Select(c => c.FromUser)
                .FirstOrDefault();

            var request = currentUser.FriendRequests.FirstOrDefault(c => c.Id == requestId);
            if (request == null)
            {
                return this.NotFound();
            }

            request.FriendRequestStatus = FriendRequestStatus.Approved;
            currentUser.Friends.Add(existingUser);

            this.Data.SaveChanges();
            return this.Ok();

        }
        [HttpPut]
        [Route("requests/{requestId}/reject")]
        public IHttpActionResult RejectFriendRequest(int requestId)
        {
            var currentUserId = this.UserIdProvider.GetUserId();
            var user = this.Data.Users.Find(currentUserId);

            var request = user
                .FriendRequests
                .FirstOrDefault(c => c.Id == requestId);
            if (request == null)
            {
                return this.NotFound();
            }

            request.FriendRequestStatus = FriendRequestStatus.Declined;

            this.Data.SaveChanges();
            return this.Ok();
        }
    }
}
