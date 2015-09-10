namespace SocialNetwork.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Data.Data;
    using Data.Interfaces;
    using Infrastructure;
    using Models.BindingModels;
    using Models.BindingModels.User;
    using Models.ViewModels;
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

        [HttpPatch]
        [Route]
        public IHttpActionResult EditUserProfile(EditProfilBindingModel bindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (bindingModel == null)
            {
                return this.BadRequest("Invalid data.");
            }

            var userId = this.UserIdProvider.GetUserId();
            var user = this.Data.Users.Find(userId);

            if (bindingModel.Email != null)
            {
                bool isEmailFree = this.Data
                    .Users
                    .All()
                    .FirstOrDefault(u => u.Email == bindingModel.Email) == null;
                if (!isEmailFree)
                {
                    return this.BadRequest("Email is already taken by another user.");
                }

                user.Email = bindingModel.Email;
            }

            if (bindingModel.Name != null)
            {
                user.Name = bindingModel.Name;
            }

            if (bindingModel.Gender != null)
            {
                user.Gender = bindingModel.Gender;
            }

            if (bindingModel.ProfileImageData != null && this.IsValidBase64Format(bindingModel.ProfileImageData))
            {
                user.ProfileImageData = string.Format(
                    "{0}{1}", "data:image/jpg;base64,", bindingModel.ProfileImageData);
            }

            if (bindingModel.CoverImageData != null && this.IsValidBase64Format(bindingModel.CoverImageData))
            {
                user.CoverImageData = string.Format(
                    "{0}{1}", "data:image/jpg;base64,", bindingModel.CoverImageData);
            }
            
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
                .Select(UserViewModelMinified.Create(user));

            return this.Ok(friends);
        }

        [HttpGet]
        [Route("friends/preview")]
        public IHttpActionResult GetFriendsPreviewData()
        {
            var currentUserId = this.UserIdProvider.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);
            var friends = currentUser.Friends
                .AsQueryable()
                .Take(6)
                .Select(UserViewModelMinified.Create(currentUser));

            return this.Ok(new
            {
                totalCount = currentUser.Friends.Count,
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
                .Where(r => r.FriendRequestStatus == FriendRequestStatus.AwaitingApproval)
                .AsQueryable()
                .Select(FriendRequestViewModel.Create(currentUser));

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
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (bindingModel == null)
            {
                return this.BadRequest("Invalid data.");
            }

            var currentUserId = this.UserIdProvider.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);
            var posts = this.Data
                .Posts
                .All()
                .Where(p => p.Author.Friends.Any(f => f.Id == currentUserId) ||
                    p.WallOwner.Friends.Any(f => f.Id == currentUserId))
                .OrderByDescending(p => p.PostedOn)
                .Skip(bindingModel.StartPostNumber)
                .Take(bindingModel.PostsCount)
                .Select(PostViewModel.Create(currentUser));

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
            existingUser.Friends.Add(currentUser);

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
