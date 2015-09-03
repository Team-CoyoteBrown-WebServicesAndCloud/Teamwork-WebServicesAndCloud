namespace SocialNetwork.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Data.Data;
    using Data.Interfaces;
    using Infrastructure;
    using Models.BindingModels.Comments;
    using Models.BindingModels.Post;
    using Models.ViewModels.Comments;
    using Models.ViewModels.Post;
    using SocialNetwork.Models;
    using UserSessionUtils;

    [SessionAuthorize]
    [RoutePrefix("api/Posts")]
    public class PostsController : BaseApiController
    {
        public PostsController()
            : this(new SocialNetworkData(), new AspNetUserIdProvider())
        {
        }

        public PostsController(ISocialNetworkData data, IUserIdProvider userIdProvider)
            : base(data, userIdProvider)
        {
        }

        [HttpPost]
        [AllowAnonymous]
        [Route]
        public IHttpActionResult AddPost(AddNewPostBindingModel bindingModel)
        {
            if (bindingModel == null)
            {
                return this.BadRequest("Invalid data!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var existingWallOwner = this.Data
                .Users
                .All()
                .FirstOrDefault(u => u.UserName == bindingModel.Username);           
            if (existingWallOwner == null)
            {
                return this.BadRequest("No such user!");
            }

            var currentUserId = this.UserIdProvider.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);
            if ((!currentUser.Friends.Contains(existingWallOwner)) && (currentUserId != existingWallOwner.Id))
            {
                return this.BadRequest("You have no permissions to make this post.");
            }

            var post = new Post()
            {
                Content = bindingModel.Content,
                PostedOn = DateTime.Now,
                AuthorId = currentUserId,
                WallOwnerId = existingWallOwner.Id
            };

            this.Data.Posts.Add(post);
            this.Data.SaveChanges();

            var postViewModel = AddPostViewModel.Create(post, currentUser);
            
            return this.Ok(postViewModel);
        }

        [HttpPut]
        [AllowAnonymous]
        [Route("{postId}")]
        public IHttpActionResult EditPost(EditPostBindingModel bindingModel, int postId)
        {
            if (bindingModel == null)
            {
                return this.BadRequest("Invalid data!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var post = this.Data.Posts.Find(postId);
            if (post == null)
            {
                return this.NotFound();
            }

            var currentUserId = this.UserIdProvider.GetUserId();
            if (post.AuthorId != currentUserId)
            {
                return this.BadRequest("You have no permissions to edit this post.");
            }

            post.Content = bindingModel.Content;

            Data.SaveChanges();

            return this.Ok();
        }

        [HttpDelete]
        [AllowAnonymous]
        [Route("{postId}")]

        public IHttpActionResult DeletePost(int postId)
        {
            var post = this.Data.Posts.Find(postId);
            if (post == null)
            {
                return this.BadRequest("No such post!");
            }

            var currentUserId = this.UserIdProvider.GetUserId();
            if (post.AuthorId != currentUserId && post.WallOwnerId != currentUserId)
            {
                return this.BadRequest("You have no permissions to delete this post.");
            }

            this.Data.Posts.Delete(post);
            this.Data.SaveChanges();
            
            return this.Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("{postId}/comments")]
        public IHttpActionResult AddNewComment(AddCommentBindingModel bindingModel, int postId)
        {
            if (bindingModel == null)
            {
                return this.BadRequest("Invalid data!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentUserId = this.UserIdProvider.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);

            var post = this.Data.Posts.Find(postId);
            if (post == null)
            {
                return this.NotFound();
            }

            if (!currentUser.Friends.Contains(post.Author) && !currentUser.Friends.Contains(post.WallOwner))
            {
                return this.BadRequest("Unable to post comment. You can comment only on your friend's posts or post made on their wall.");
            }

            Comment comment = new Comment
            {
                Content = bindingModel.CommentContent,
                PostedOn = DateTime.Now,
                PostId = postId,
                AuthorId = currentUserId
            };

            post.Comments.Add(comment);
            this.Data.SaveChanges();

            var commentViewModel = CommentViewModel.Create(comment, currentUser);

            return this.Ok(commentViewModel);
        }

        [HttpPut]
        [AllowAnonymous]
        [Route("{postId}/comments/{commentId}")]
        public IHttpActionResult EditComment(AddCommentBindingModel bindingModel, int postId, int commentId)
        {
            if (bindingModel == null)
            {
                return this.BadRequest("Invalid data!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var post = this.Data.Posts.Find(postId);
            if (post == null)
            {
                return this.NotFound();
            }

            var comment = this.Data.Comments.Find(commentId);
            if (comment == null)
            {
                return this.NotFound();
            }

            var currentUserId = this.UserIdProvider.GetUserId();
            if (comment.AuthorId != currentUserId)
            {
                return this.BadRequest("This comment is not yours! You have no permission to edit it.");
            }

            comment.Content = bindingModel.CommentContent;
            Data.SaveChanges();

            return this.Ok();
        }

        [HttpDelete]
        [AllowAnonymous]
        [Route("{postId}/comments/{commentId}")]
        public IHttpActionResult DeleteComment(int postId, int commentId)
        {
            var post = this.Data.Posts.Find(postId);
            if (post == null)
            {
                return this.NotFound();
            }

            var comment = this.Data.Comments.Find(commentId);
            if (comment == null)
            {
                return this.NotFound();
            }

            var currentUserId = this.UserIdProvider.GetUserId();
            if (comment.AuthorId != currentUserId && comment.Post.WallOwnerId != currentUserId)
            {
                return this.BadRequest("You have no permission to delete this comment.");
            }

            post.Comments.Remove(comment);
            this.Data.SaveChanges();

            return this.Ok();
        }
    }
}
