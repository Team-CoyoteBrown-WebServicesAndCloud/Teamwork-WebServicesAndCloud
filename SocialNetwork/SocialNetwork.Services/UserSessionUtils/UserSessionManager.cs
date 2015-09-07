namespace SocialNetwork.Services.UserSessionUtils
{
    using System;
    using System.Linq;
    using Data.Data;
    using Data.Interfaces;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin;
    using SocialNetwork.Models;   

    public class UserSessionManager
    {
        private static readonly TimeSpan DefaultSessionTimeout = new TimeSpan(0, 24, 0, 0);

        public UserSessionManager(IOwinContext owinContext)
            : this(owinContext, new SocialNetworkData())
        {
        }

        public UserSessionManager(IOwinContext owinContext, ISocialNetworkData data)
        {
            this.OwinContext = owinContext;
            this.Data = data;
        }

        protected IOwinContext OwinContext { get; set; }
        protected ISocialNetworkData Data { get; private set; }

        public void CreateUserSession(string username, string authenticationToken)
        {
            var userId = this.Data.Users.All().First(u => u.UserName == username).Id;
            var userSession = new UserSession
            {
                OwnerUserId = userId,
                AuthenticationToken = authenticationToken
            };

            this.Data.UserSessions.Add(userSession);

            userSession.ExpirationDateTime = DateTime.Now + DefaultSessionTimeout;

            this.Data.SaveChanges();
        }

        public void InvalidateUserSession()
        {
            var authenticationToken = this.GetCurrentBearerAuthrorizationToken();
            if (authenticationToken != null)
            {
                authenticationToken = authenticationToken.Substring(7);
            }

            var currentUserId = this.GetCurrentUserId();
            var userSession = this.Data
                .UserSessions
                .All()
                .FirstOrDefault(s => s.AuthenticationToken == authenticationToken && s.OwnerUserId == currentUserId);
            if (userSession != null)
            {
                this.Data.UserSessions.Delete(userSession);
                this.Data.SaveChanges();
            }
        }

        public bool ReValidateSession()
        {
            var authorizationToken = this.GetCurrentBearerAuthrorizationToken();
            if (authorizationToken != null)
            {
                authorizationToken = authorizationToken.Substring(7);
            }

            var currentUserId = this.GetCurrentUserId();
            var userSession = this.Data
                .UserSessions
                .All()
                .FirstOrDefault(s => s.AuthenticationToken == authorizationToken && s.OwnerUserId == currentUserId);
            if (userSession == null)
            {
                return false;
            }

            if (userSession.ExpirationDateTime < DateTime.Now)
            {
                return false;
            }

            userSession.ExpirationDateTime = DateTime.Now + DefaultSessionTimeout;

            this.Data.SaveChanges();

            return true;
        }

        public void DeleteExpiredSession()
        {
            var expiredSessions = this.Data
                .UserSessions
                .All()
                .Where(session => session.ExpirationDateTime < DateTime.Now);

            foreach (var session in expiredSessions)
            {
                this.Data.UserSessions.Delete(session);
            }
        }

        private string GetCurrentBearerAuthrorizationToken()
        {
            string authenticationToken = null;
            if (this.OwinContext.Request.Headers["Authorization"] != null)
            {
                authenticationToken = this.OwinContext.Request.Headers["Authorization"];
            }

            return authenticationToken;
        }

        private string GetCurrentUserId()
        {
            var currentUser = this.OwinContext.Authentication.User.Identity;
            if (currentUser == null)
            {
                return null;
            }

            return currentUser.GetUserId();
        }
    }
}