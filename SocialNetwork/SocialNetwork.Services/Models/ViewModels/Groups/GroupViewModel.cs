namespace SocialNetwork.Services.Models.ViewModels.Groups
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Post;
    using SocialNetwork.Models;
    using User;

    public class GroupViewModel : BaseGroupViewModel
    {
        public string Description { get; set; }

        public string CoverImageData { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<UserViewModelMinified> Admins { get; set; }

        public IEnumerable<UserViewModelMinified> Members { get; set; }

        public IEnumerable<GroupPostViewModel> Posts { get; set; }

        public static Expression<Func<Group, GroupViewModel>> Create(ApplicationUser currentUser)
        {
            return group => new GroupViewModel
            {
                Id = group.Id,
                Name = group.Name,
                CoverImageData = group.CoverImageData,
                Type = group.Type.ToString(),
                CreatedOn = group.CreatedOn,
                Description = group.Description,
                Admins = group.Admins
                    .AsQueryable()
                    .Select(UserViewModelMinified.Create(currentUser)),
                Members = group.Members
                    .AsQueryable()
                    .Select(UserViewModelMinified.Create(currentUser)),
                Posts = group.Posts
                    .AsQueryable()
                    .Select(GroupPostViewModel.Create(currentUser))
            };
        }

        public static GroupViewModel ConvertTo(Group group, ApplicationUser currentUser)
        {
            GroupViewModel viewModel = new GroupViewModel
            {
                Id = group.Id,
                Name = group.Name,
                CoverImageData = group.CoverImageData,
                Type = group.Type.ToString(),
                CreatedOn = group.CreatedOn,
                Description = group.Description,
                Admins = group.Admins
                    .AsQueryable()
                    .Select(UserViewModelMinified.Create(currentUser)),
                Members = group.Members
                    .AsQueryable()
                    .Select(UserViewModelMinified.Create(currentUser)),
                Posts = group.Posts
                    .AsQueryable()
                    .Select(GroupPostViewModel.Create(currentUser))
            };

            return viewModel;
        }
    }
}