namespace SocialNetwork.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Data.Data;
    using Data.Interfaces;
    using Infrastructure;
    using Microsoft.AspNet.Identity;
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
        [Route("{postId}/likes")]
        public IHttpActionResult LikePost(int postId)
        {
            var currentUserId = this.UserIdProvider.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);
            var post = this.Data.Posts.Find(postId);
            if (post == null)
            {
                return this.NotFound();
            }

            if (!currentUser.Friends.Contains(post.Author) && !currentUser.Friends.Contains(post.WallOwner))
            {
                return this.BadRequest("Unable to like that post. You can like posts of your friends or on their wall.");
            }

            if (post.Likes.Any(p => p.UserId == currentUserId))
            {
                return this.BadRequest("You have already like this post.");
            }

            PostLike postLike = new PostLike
            {
                PostId = post.Id,
                UserId = currentUserId
            };

            this.Data.PostLikes.Add(postLike);
            this.Data.SaveChanges();

            return this.Ok();
        }

        [HttpDelete]
        [Route("{postId}/likes")]
        public IHttpActionResult UnlikePost(int postId)
        {
            var currentUserId = this.UserIdProvider.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);
            var post = this.Data.Posts.Find(postId);
            if (post == null)
            {
                return this.NotFound();
            }

            if (!currentUser.Friends.Contains(post.Author) && !currentUser.Friends.Contains(post.WallOwner))
            {
                return this.BadRequest("Unable to unlike that post. You can unlike posts of your friends or on their wall.");
            }

            var postLike = post.Likes.FirstOrDefault(p => p.UserId == currentUserId);
            if (postLike == null)
            {
                return this.BadRequest("This post has no like from you.");
            }

            this.Data.PostLikes.Delete(postLike);
            this.Data.SaveChanges();

            return this.Ok();
        }

        [HttpPost]
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

            this.Data.Comments.Delete(comment);
            foreach (var like in comment.Likes)
            {
                this.Data.CommentLikes.Delete(like);
            }

            foreach (var reply in comment.Replies)
            {
                foreach (var replyLikes in reply.Likes)
                {
                    this.Data.CommentLikes.Delete(replyLikes);
                }

                this.Data.CommentReplies.Delete(reply);
            }

            this.Data.SaveChanges();

            return this.Ok();
        }

        [HttpPost]
        [Route("{postId}/comments/{commentId}/likes")]
        public IHttpActionResult LikeComment(int postId, int commentId)
        {
            var currentUserId = this.UserIdProvider.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);
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

            if (!currentUser.Friends.Contains(post.Author) && !currentUser.Friends.Contains(post.WallOwner))
            {
                return this.BadRequest("Unable to like that comment. You can like comments of your friends post or on their wall posts.");
            }

            if (comment.Likes.Any(c => c.UserId == currentUserId))
            {
                return this.BadRequest("You have already like this comment.");
            }

            CommentLike commentLike = new CommentLike
            {
                CommentId = comment.Id,
                UserId = currentUserId
            };

            this.Data.CommentLikes.Add(commentLike);
            this.Data.SaveChanges();

            return this.Ok();
        }

        [HttpDelete]
        [Route("{postId}/comments/{commentId}/likes")]
        public IHttpActionResult UnlikeComment(int postId, int commentId)
        {
            var currentUserId = this.UserIdProvider.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);
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

            if (!currentUser.Friends.Contains(post.Author) && !currentUser.Friends.Contains(post.WallOwner))
            {
                return this.BadRequest("Unable to unlike that comment. You can unlike comments of your friends posts or on their wall posts.");
            }

            var commentLike = comment.Likes.FirstOrDefault(c => c.UserId == currentUserId);
            if (commentLike == null)
            {
                return this.BadRequest("This comment has no like from you.");
            }

            this.Data.CommentLikes.Delete(commentLike);
            this.Data.SaveChanges();

            return this.Ok();
        }

        [HttpPost]
        [Route("{postId}/comments/{commentId}")]
        public IHttpActionResult ReplyComment(AddCommentBindingModel bindingModel, int postId, int commentId)
        {
            var currentUserId = this.UserIdProvider.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);
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

            if (!currentUser.Friends.Contains(post.Author) && !currentUser.Friends.Contains(post.WallOwner))
            {
                return this.BadRequest("Unable to comment. You can comment only posts of your friends or posts on their wall.");
            }

            CommentReply commentReply = new CommentReply
            {
                AuthorId = currentUserId,
                CommentId = comment.Id,
                Content = bindingModel.CommentContent,
                RepliedOn = DateTime.Now
            };

            comment.Replies.Add(commentReply);
            this.Data.SaveChanges();

            return this.Ok();
        }
    }
}
