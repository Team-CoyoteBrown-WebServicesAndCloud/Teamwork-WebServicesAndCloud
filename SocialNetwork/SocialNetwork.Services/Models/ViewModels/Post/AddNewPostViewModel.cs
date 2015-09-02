using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace SocialNetwork.Services.Models.ViewModels.Post
{
    public class AddNewPostViewModel : BasePostViewModel
    {
        public static Expression<Func<SocialNetwork.Models.Post, AddNewPostViewModel>> Create
        {
            get
            {
                return post => new AddNewPostViewModel
                {
                    Id = post.Id,
                    Content = post.Content,
                    AuthorId = post.AuthorId,
                    WallOwnerId = post.WallOwnerId
                };
            }
        }
    }
}