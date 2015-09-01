namespace SocialNetwork.Services.Controllers
{
    using System.Web.Http;
    using Data.Data;
    using Data.Interfaces;
    using Infrastructure;

    public class BaseApiController : ApiController
    {
        public BaseApiController(ISocialNetworkData data)
            : this(new SocialNetworkData(), new AspNetUserIdProvider())
        {
        }

        public BaseApiController(ISocialNetworkData data, IUserIdProvider userIdProvider)
        {
            this.Data = data;
            this.UserIdProvider = userIdProvider;
        }

        protected IUserIdProvider UserIdProvider { get; set; }

        protected ISocialNetworkData Data { get; private set; }
    }
}