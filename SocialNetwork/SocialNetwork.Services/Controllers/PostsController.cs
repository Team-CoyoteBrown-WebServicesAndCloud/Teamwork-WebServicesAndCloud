namespace SocialNetwork.Services.Controllers
{
    using System.Web.Http;
    using Data.Interfaces;
    using Infrastructure;
    using UserSessionUtils;

    [SessionAuthorize]
    [RoutePrefix("api/Posts")]
    public class PostsController : BaseApiController
    {
        public PostsController(ISocialNetworkData data)
            : base(data)
        {
        }

        public PostsController(ISocialNetworkData data, IUserIdProvider userIdProvider)
            : base(data, userIdProvider)
        {
        }


    }
}
