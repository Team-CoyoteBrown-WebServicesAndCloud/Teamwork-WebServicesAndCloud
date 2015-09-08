namespace SocialNetwork.Services.Models.ViewModels.Post
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Comments;
    using SocialNetwork.Models;
    using User;

    public class PostViewModel
    {
        public int Id { get; set; }

        public static ApplicationUser CurrentUser { get; set; }

        public UserViewModelMinified Author { get; set; }

        public UserViewModelMinified WallOwner { get; set; }

        public string PostContent { get; set; }

        public DateTime Date { get; set; }

        public int LikesCount { get; set; }

        public bool Liked { get; set; }

        public int TotalCommentsCount { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public static Expression<Func<Post, PostViewModel>> Create
        {
            get
            {
                return post => new PostViewModel
                {
                    Id = post.Id,
                    Author = new UserViewModelMinified
                    {
                        Id = post.AuthorId,
                        Name = post.Author.Name,
                        Username = post.Author.UserName,
                        IsFriend = post.Author.Friends.Any(f => f.Id == CurrentUser.Id),
                        Gender = post.Author.Gender,
                        ProfileImageData = post.Author.ProfileImageData,
                    },
                    WallOwner = new UserViewModelMinified
                    {
                        Id = post.WallOwnerId,
                        Name = post.WallOwner.Name,
                        Username = post.WallOwner.UserName,
                        IsFriend = post.Author.Friends.Any(f => f.Id == CurrentUser.Id),
                        Gender = post.WallOwner.Gender,
                        ProfileImageData = post.WallOwner.ProfileImageData,
                    },
                    PostContent = post.Content,
                    Date = post.PostedOn,
                    LikesCount = post.Likes.Count,
                    Liked = post.Likes.Any(l => l.UserId == CurrentUser.Id),
                    TotalCommentsCount = post.Comments.Count,
                    Comments = post.Comments
                        .OrderByDescending(c => c.PostedOn)
                        .Take(4)
                        .Select(comment => new CommentViewModel
                        {
                            Id = comment.Id,
                            Author = new UserViewModelMinified
                            {
                                Id = comment.AuthorId,
                                Name = comment.Author.Name,
                                Username = comment.Author.UserName,
                                IsFriend = comment.Author.Friends.Any(f => f.Id == CurrentUser.Id),
                                Gender = comment.Author.Gender,
                                ProfileImageData = comment.Author.ProfileImageData,
                            },
                            Date = comment.PostedOn,
                            CommentContent = comment.Content,
                            LikesCount = comment.Likes.Count,
                            Liked = comment.Likes.Any(l => l.UserId == CurrentUser.Id),
                            CommentReplies = comment.Replies
                                .OrderByDescending(cr => cr.RepliedOn)
                                .Select(reply => new CommentReplyViewModel
                                {
                                    Id = reply.Id,
                                    Author = new UserViewModelMinified
                                    {
                                        Id = reply.AuthorId,
                                        Name = reply.Author.Name,
                                        Username = reply.Author.UserName,
                                        IsFriend = reply.Author.Friends.Any(f => f.Id == CurrentUser.Id),
                                        Gender = reply.Author.Gender,
                                        ProfileImageData = reply.Author.ProfileImageData,
                                    },
                                    Date = reply.RepliedOn,
                                    CommentContent = reply.Content,
                                    LikesCount = reply.Likes.Count,
                                    Liked = reply.Likes.Any(l => l.UserId == CurrentUser.Id),
                                })
                        })
                };
            }
        }

        //public static PostViewModel ConvertTo(Post post, ApplicationUser currentUser)
        //{
        //    return new PostViewModel()
        //    {
        //        Id = post.Id,
        //        Author = new UserViewModelMinified
        //        {
        //            Id = post.AuthorId,
        //            Name = post.Author.Name,
        //            Username = post.Author.UserName,
        //            Gender = post.Author.Gender,
        //            ProfileImageData = post.Author.ProfileImageData,
        //        },
        //        WallOwner = new UserViewModelMinified
        //        {
        //            Id = post.WallOwnerId,
        //            Name = post.WallOwner.Name,
        //            Username = post.WallOwner.UserName,
        //            Gender = post.WallOwner.Gender,
        //            ProfileImageData = post.WallOwner.ProfileImageData,
        //        },
        //        PostContent = post.Content,
        //        Date = post.PostedOn,
        //        LikesCount = post.Likes.Count,
        //        Liked = post.Likes.Any(l => l.UserId == currentUser.Id),
        //        TotalCommentsCount = post.Comments.Count,
        //        Comments = post.Comments
        //            .Reverse()
        //            .Take(4)
        //            .Select(c => new CommentViewModel
        //            {
        //                Id = c.Id,
        //                Author = new UserViewModelMinified
        //                {
        //                    Id = c.AuthorId,
        //                    Name = post.Author.Name,
        //                    Username = post.Author.UserName,
        //                    Gender = post.Author.Gender,
        //                    ProfileImageData = post.Author.ProfileImageData,
        //                },
        //                Date = c.PostedOn,
        //            })
        //    };
        //}
    }
}