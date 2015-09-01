namespace SocialNetwork.Services.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Script.Serialization;
    using Data.Data;
    using Data.Interfaces;
    using Infrastructure;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Testing;
    using Models.BindingModels;
    using SocialNetwork.Models;
    using UserSessionUtils;

    [SessionAuthorize]
    [RoutePrefix("api/users")]
    public class UsersController : BaseApiController
    {
        private readonly ApplicationUserManager userManager;

        public UsersController()
            : this(new SocialNetworkData(), new AspNetUserIdProvider())
        {
        }

        public UsersController(ISocialNetworkData data, IUserIdProvider userIdProvider)
            : base(data, userIdProvider)
        {
            this.userManager = new ApplicationUserManager(
               new UserStore<ApplicationUser>(new SocialNetworkContext()));
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager;
            }
        }

        private IAuthenticationManager Authentication
        {
            get
            {
                return this.Request.GetOwinContext().Authentication;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IHttpActionResult> LoginUser(LoginUserBindingModel bindingModel)
        {
            if (this.UserIdProvider.GetUserId() != null)
            {
                return this.BadRequest("User is already logged in.");
            }

            if (!this.ModelState.IsValid || bindingModel == null)
            {
                return this.BadRequest("Invalid user data");
            }

            var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", bindingModel.Username),
                new KeyValuePair<string, string>("password", bindingModel.Password)
            };

            var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
            var testServer = TestServer.Create<Startup>();
            var tokenServiceResponse = await testServer.HttpClient.PostAsync("/api/Token", requestParamsFormUrlEncoded);
            if (tokenServiceResponse.StatusCode == HttpStatusCode.OK)
            {
                var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                var jsSerializer = new JavaScriptSerializer();
                var responseData = jsSerializer.Deserialize<Dictionary<string, string>>(responseString);
                var authenticationToken = responseData["access_token"];
                var username = responseData["userName"];
                var owinContext = this.Request.GetOwinContext();
                var userSessionManager = new UserSessionManager(owinContext);
                
                userSessionManager.CreateUserSession(username, authenticationToken);
                userSessionManager.DeleteExpiredSession();
            }

            return this.ResponseMessage(tokenServiceResponse);
        }
    }
}