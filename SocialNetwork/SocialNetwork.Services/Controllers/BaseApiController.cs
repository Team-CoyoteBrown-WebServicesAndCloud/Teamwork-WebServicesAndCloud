﻿namespace SocialNetwork.Services.Controllers
{
    using System.Web.Http;
    using Data.Data;
    using Data.Interfaces;
    using Infrastructure;
    using Microsoft.AspNet.Identity;

    public class BaseApiController : ApiController
    {
        public BaseApiController()
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

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return this.InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error);
                    }
                }

                if (this.ModelState.IsValid)
                {
                    return this.BadRequest();
                }

                return this.BadRequest(this.ModelState);
            }

            return null;
        }
    }
}