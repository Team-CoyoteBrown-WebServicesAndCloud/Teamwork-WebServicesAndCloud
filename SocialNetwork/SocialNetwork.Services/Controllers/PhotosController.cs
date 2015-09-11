namespace SocialNetwork.Services.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Data.Data;
    using Data.Interfaces;
    using Infrastructure;
    using Models.ViewModels.Photos;
    using SocialNetwork.Models;
    using UserSessionUtils;

    [SessionAuthorize]
    [RoutePrefix("api/photos")]
    public class PhotosController : BaseApiController
    {
        public PhotosController()
            : base(new SocialNetworkData())
        {
        }

        public PhotosController(ISocialNetworkData data, IUserIdProvider userIdProvider)
            : base(data, userIdProvider)
        {
        }

        [HttpGet]
        [Route("{photoId}/{username}")]
        public IHttpActionResult GetPhotoById(int photoId, string username)
        {
            var currentUserId = this.UserIdProvider.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);
            var wantedUser = this.Data
               .Users
               .All()
               .FirstOrDefault(u => u.UserName == username);
            if (wantedUser == null)
            {
                return this.NotFound();
            }

            if (!this.HasAuthorizationForDetailedInfo(wantedUser, currentUserId))
            {
                return this.Unauthorized();
            }

            var photo = this.Data.Photos
                .All()
                .Where(p => p.Id == photoId)
                .Select(PhotoViewModel.Create(currentUser))
                .FirstOrDefault();
            if (photo == null)
            {
                return this.NotFound();
            }

            return this.Ok(photo);
        }

        [HttpPost]
        [Route("{photoId}/likes")]
        public IHttpActionResult LikePost(int photoId)
        {
            var currentUserId = this.UserIdProvider.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);
            var photo = this.Data.Photos.Find(photoId);
            if (photo == null)
            {
                return this.NotFound();
            }

            if (!this.HasAuthorization(currentUser, photo))
            {
                return this.BadRequest("Unable to like that photo. You can like photos of your friends only.");
            }

            if (photo.Likes.Any(p => p.UserId == currentUserId))
            {
                return this.BadRequest("You have already like this photo.");
            }

            PhotoLike photoLike = new PhotoLike
            {
                PhotoId = photo.Id,
                UserId = currentUserId
            };

            this.Data.PhotoLikes.Add(photoLike);
            this.Data.SaveChanges();

            return this.Ok();
        }

        [HttpDelete]
        [Route("{photoId}/likes")]
        public IHttpActionResult UnlikePost(int photoId)
        {
            var currentUserId = this.UserIdProvider.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);
            var photo = this.Data.Photos.Find(photoId);
            if (photo == null)
            {
                return this.NotFound();
            }

            if (!this.HasAuthorization(currentUser, photo))
            {
                return this.BadRequest("Unable to unlike that photo. You can like and unlike photos of your friends only.");
            }

            var photoLike = photo.Likes.FirstOrDefault(p => p.UserId == currentUserId);
            if (photoLike == null)
            {
                return this.BadRequest("This photo has no like from you.");
            }

            this.Data.PhotoLikes.Delete(photoLike);
            this.Data.SaveChanges();

            return this.Ok();
        }

        private bool HasAuthorization(ApplicationUser currentUser, Photo photo)
        {
            if (currentUser.Friends.Contains(photo.PhotoOwner) ||
                   photo.PhotoOwnerId == currentUser.Id)
            {
                return true;
            }

            return false;
        }
    }
}
