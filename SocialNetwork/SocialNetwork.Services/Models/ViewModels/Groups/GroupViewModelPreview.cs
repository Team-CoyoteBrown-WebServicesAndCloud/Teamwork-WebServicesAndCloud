namespace SocialNetwork.Services.Models.ViewModels.Groups
{
    using System;
    using System.Linq.Expressions;
    using SocialNetwork.Models;

    public class GroupViewModelPreview : BaseGroupViewModel
    {
        public static Expression<Func<Group, GroupViewModelPreview>> Create
        {
            get
            {
                return group => new GroupViewModelPreview
                {
                    Id = group.Id,
                    Name = group.Name,
                    Type = group.Type.ToString()
                };
            }
        }
    }
}