namespace SocialNetwork.Services.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Data.Data;
    using Data.Interfaces;
    using Infrastructure;
    using Models.BindingModels;

    [Authorize]
    public class ProfileController : BaseApiController
    {
        private IUserIdProvider idProvider;
        public ProfileController(ISocialNetworkData data, IUserIdProvider idProvider)
            : base(data)
        {
            this.idProvider = idProvider;
        }

        public ProfileController()
            : this(new SocialNetworkContextData(), new AspNetUserIdProvider())
        {

        }
        [Route("api/me")]
        [HttpGet]
        public IHttpActionResult GetCurrentUserData()
        {
            var userId = this.idProvider.GetUserId();
            var user = this.Data.Users.Find(userId);
            return this.Ok(user);
        }
        [Route("api/me")]
        [HttpPut]
        public IHttpActionResult EditUserProfil(EditProfilBindingModel model)
        {
            var userId = this.idProvider.GetUserId();
            var user = this.Data.Users.Find(userId);
            user.Name = model.Name;
            user.Email = model.Name;
            user.Gender = model.Gender;
            this.Data.Users.Update(user);
            this.Data.SaveChanges();
            return this.Ok();
        }
        [Route("api/me/friends")]
        [HttpGet]
        public IHttpActionResult GetFriendsData()
        {
            var userId = this.idProvider.GetUserId();
            var user = this.Data.Users.Find(userId);
            var friends = user.Friends;
            return this.Ok(friends);
        }
    }
}
