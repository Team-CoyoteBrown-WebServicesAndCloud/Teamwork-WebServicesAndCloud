namespace SocialNetwork.Services.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
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
    using Models.BindingModels.User;
    using Models.ViewModels.User;
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
            get { return this.userManager; }
        }

        private IAuthenticationManager Authentication
        {
            get { return this.Request.GetOwinContext().Authentication; }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> RegisterUser(RegisterUserBindingModel bindingModel)
        {
            if (this.UserIdProvider.GetUserId() != null)
            {
                return this.BadRequest("User is already logged in.");
            }

            if (bindingModel == null)
            {
                return this.BadRequest("Invalid user data");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var existingEmail = this.Data
                .Users
                .All()
                .FirstOrDefault(u => u.Email == bindingModel.Email);
            if (existingEmail != null)
            {
                return this.BadRequest("A user with the same email already exists.");
            }

            var newUser = new ApplicationUser
            {
                UserName = bindingModel.Username,
                Name = bindingModel.Name,
                Email = bindingModel.Email,
                Age = bindingModel.Age,
                Gender = bindingModel.Gender
            };

            var identityResult = await this.UserManager.CreateAsync(newUser, bindingModel.Password);
            if (!identityResult.Succeeded)
            {
                return this.GetErrorResult(identityResult);
            }

            var loginResult = await this.LoginUser(new LoginUserBindingModel
            {
                Username = bindingModel.Username,
                Password = bindingModel.Password
            });

            return loginResult;
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

        [HttpPost]
        [Route("logout")]
        public IHttpActionResult Logout()
        {
            this.Authentication.SignOut(DefaultAuthenticationTypes.ExternalBearer);
            var owinContext = this.Request.GetOwinContext();
            var userSessionManager = new UserSessionManager(owinContext);

            userSessionManager.InvalidateUserSession();

            return this.Ok("Logout successful");
        }

        [HttpGet]
        [Route("search")]
        public IHttpActionResult SearchUsers([FromUri] SearchBindingModel bindingModel)
        {
            var users = this.Data
                .Users
                .All()
                .OrderBy(u => u.Name)
                .ThenBy(u => u.UserName)
                .Where(u => u.Name.Contains(bindingModel.SearchWord))
                .Take(5)
                .Select(UserViewModelMinified.Create);

            return this.Ok(users);
        }
    }
}