namespace SocialNetwork.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Data.Data;
    using Data.Interfaces;
    using Infrastructure;
    using Models.BindingModels.Groups;
    using Models.ViewModels.Groups;
    using SocialNetwork.Models;
    using UserSessionUtils;

    [SessionAuthorize]
    [RoutePrefix("api/groups")]
    public class GroupsController : BaseApiController
    {
        public GroupsController()
            : base(new SocialNetworkData(), new AspNetUserIdProvider())
        {
        }

        public GroupsController(ISocialNetworkData data, IUserIdProvider userIdProvider)
            : base(data, userIdProvider)
        {
        }

        [HttpGet]
        [Route("{groupId}")]
        public IHttpActionResult GetGroupById(int groupId)
        {
            var group = this.Data.Groups.Find(groupId);
            if (group == null)
            {
                return this.BadRequest("The group does not exist");
            }

            var currentUserId = this.UserIdProvider.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);

            var groupViewModel = GroupViewModel.ConvertTo(group, currentUser);

            return this.Ok(groupViewModel);
        }

        [HttpPost]
        [Route]
        public IHttpActionResult CreateGroup(CreateGroupBindingModel bindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (bindingModel == null)
            {
                return this.BadRequest("Invalid data");
            }

            var currentUserId = this.UserIdProvider.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);

            Group group = new Group
            {
                Name = bindingModel.Name,
                Description = bindingModel.Description,
                CoverImageData = null,
                CreatedOn = DateTime.Now
            };

            if (bindingModel.CoverImageData != null && this.IsValidBase64Format(bindingModel.CoverImageData))
            {
                group.CoverImageData = string.Format(
                    "{0}{1}", "data:image/jpg;base64,", bindingModel.CoverImageData);
            }

            group.Members.Add(currentUser);
            group.Admins.Add(currentUser);
            currentUser.Groups.Add(group);

            this.Data.Groups.Add(group);
            this.Data.SaveChanges();

            return this.Ok();
        }

        [HttpPost]
        [Route("{groupId}/join")]
        public IHttpActionResult JoinGroup(int groupId)
        {
            var group = this.Data.Groups.Find(groupId);
            if (group == null)
            {
                return this.BadRequest("The group does not exist");
            }

            var currentUserId = this.UserIdProvider.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);
            if (group.Members.Any(m => m.Id == currentUserId))
            {
                return this.BadRequest("The user is already in this group");
            }
            
            group.Members.Add(currentUser);
            currentUser.Groups.Add(group);

            this.Data.SaveChanges();

            return this.Ok();
        }
    }
}
