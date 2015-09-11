namespace SocialNetwork.Services.Models.ViewModels.Post
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Comments;
    using Groups;
    using SocialNetwork.Models;
    using User;

    public class GroupPostViewModel : BasePostViewModel
    {
        public UserViewModelMinified Author { get; set; }

        public GroupViewModelMinified Group { get; set; }

        public DateTime Date { get; set; }

        public int LikesCount { get; set; }

        public bool Liked { get; set; }

        public int TotalCommentsCount { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public static Expression<Func<GroupPost, GroupPostViewModel>> Create(ApplicationUser currentUser)
        {
            string currentUserId = currentUser.Id;
            return post => new GroupPostViewModel
            {
                Id = post.Id,
                Author = new UserViewModelMinified
                {
                    Id = post.AuthorId,
                    Name = post.Author.Name,
                    Username = post.Author.UserName,
                    IsFriend = post.Author.Friends.Any(f => f.Id == currentUserId),
                    Gender = post.Author.Gender,
                    ProfileImageData = post.Author.ProfileImageData,
                },
                Group = new GroupViewModelMinified
                {
                    Id = post.GroupId,
                    Name = post.Group.Name,
                    CoverImageData = post.Group.CoverImageData,
                    CreatedOn = post.Group.CreatedOn,
                    Description = post.Group.Description,
                },
                PostContent = post.Content,
                Date = post.PostedOn,
                LikesCount = post.Likes.Count,
                Liked = post.Likes.Any(l => l.UserId == currentUserId),
                TotalCommentsCount = post.Comments.Count,
                Comments = post.Comments
                    .OrderByDescending(c => c.PostedOn)
                    .Take(4)
                    .AsQueryable()
                    .Select(CommentViewModel.Create(currentUser))
            };
        }
    }
}