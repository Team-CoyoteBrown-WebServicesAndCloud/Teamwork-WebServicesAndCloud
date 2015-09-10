namespace SocialNetwork.Services.Models.ViewModels.Groups
{
    using System;
    using System.Linq.Expressions;
    using SocialNetwork.Models;

    public class GroupViewModelMinified : BaseGroupViewModel
    {
        public string Description { get; set; }

        public string CoverImageData { get; set; }

        public DateTime CreatedOn { get; set; }

        public static Expression<Func<Group, GroupViewModelMinified>> Create
        {
            get
            {
                return group => new GroupViewModelMinified
                {
                    Id = group.Id,
                    Name = group.Name,
                    Type = group.Type.ToString(),
                    Description = group.Description,
                    CoverImageData = group.CoverImageData,
                    CreatedOn = group.CreatedOn
                };
            }
        }
    }
}