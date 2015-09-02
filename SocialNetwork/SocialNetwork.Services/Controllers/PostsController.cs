namespace SocialNetwork.Services.Controllers
{
    using System.Web.Http;
    using Data.Interfaces;
    using Infrastructure;
    using UserSessionUtils;
    using SocialNetwork.Services.Models.BindingModels.Post;
    using SocialNetwork.Models;
    using System.Threading.Tasks;
    using System.Linq;
    using SocialNetwork.Data.Data;

    [SessionAuthorize]
    [RoutePrefix("api/Posts")]
    public class PostsController : BaseApiController
    {
        public PostsController()
            : this(new SocialNetworkData(), new AspNetUserIdProvider())
        {
        }

        public PostsController(ISocialNetworkData data)
            : base(data)
        {
        }

        public PostsController(ISocialNetworkData data, IUserIdProvider userIdProvider)
            : base(data, userIdProvider)
        {
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("AddNewPost")]
        public IHttpActionResult AddNewPost(AddNewPostBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Invalid data!");
            }

            var existingAuthor = this.Data
                .Users
                .All()
                .FirstOrDefault(u => u.Id == model.AuthorId);

            if (existingAuthor == null)
            {
                return this.BadRequest("No such author!");
            }

            var existingWallOwner = this.Data
                .Users
                .All()
                .FirstOrDefault(u => u.Id == model.WallOwnerId);           

            if (existingWallOwner == null)
            {
                return this.BadRequest("No such wall owner!");
            }

            var post = new Post()
                        {
                            Content = model.Content,
                            PostedOn = model.PostedOn,
                            AuthorId = model.AuthorId,
                            WallOwnerId = model.WallOwnerId
                        };

            Data.Posts.Add(post);
            Data.SaveChanges();

            return this.Ok("Posted successfuly");
        }

        [HttpDelete]
        [AllowAnonymous]
        [Route("DeletePost")]

        public IHttpActionResult DeletePost(DeletePostBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Invalid data!");
            }

            var existingAuthor = this.Data
                .Users
                .All()
                .FirstOrDefault(u => u.Id == model.AuthorId);

            if (existingAuthor == null)
            {
                return this.BadRequest("No such author!");
            }

            var existingWallOwner = this.Data
                .Users
                .All()
                .FirstOrDefault(u => u.Id == model.WallOwnerId);

            var post = this.Data
                .Posts
                .All()
                .FirstOrDefault(p=> p.Id == model.Id);

            if (existingWallOwner == null)
            {
                return this.BadRequest("No such wall owner!");
            }

            if (post.AuthorId != model.AuthorId || post.WallOwnerId != model.WallOwnerId)
            {
                return this.BadRequest("You are not the author or the wall owner of this post!");
            }

            Data.Posts.Delete(post);
            Data.SaveChanges();
            return this.Ok("Post deleted !");
        }

        [HttpPut]
        [AllowAnonymous]
        [Route("EditPost")]
        public IHttpActionResult EditPost(EditPostBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Invalid data!");
            }

            var post = this.Data
                .Posts
                .All()
                .FirstOrDefault(p => p.Id == model.Id);

            if (post == null)
            {
                return this.BadRequest("No such post!");
            }

            if (post.AuthorId != model.AuthorId)
            {
                return this.BadRequest("You are not the author and can not change the post!");
            }

            post.Content = model.Content;

            Data.SaveChanges();

            return this.Ok("Post content has been changed.");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("AddNewComment")]
        public IHttpActionResult AddNewComment(CommentBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Invalid data!");
            }

            var post = this.Data
                .Posts
                .All()
                .FirstOrDefault(p => p.Id == model.PostId);

            if (post == null)
            {
                return this.BadRequest("No such post!");
            }

            var comment = new Comment()
            {
                Content = model.Content,
                PostedOn = model.PostedOn,
                PostId = model.PostId,
                AuthorId = model.AuthorId
            };

            Data.Comments.Add(comment);
            Data.SaveChanges();

            return this.Ok("Comment has been added.");
        }

        [HttpPut]
        [AllowAnonymous]
        [Route("EditComment")]
        public IHttpActionResult EditComment(CommentBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Invalid data!");
            }

            var comment = this.Data
                   .Comments
                   .All()
                   .FirstOrDefault(c => c.Id == model.Id);

            if (comment == null)
            {
                return this.BadRequest("No such comment!");
            }

            if (comment.AuthorId != model.AuthorId)
            {
                return this.BadRequest("You have not authored this comment!");
            }

            comment.Content = model.Content;
            Data.SaveChanges();

            return this.Ok("Changes saved.");
        }
    }
}
