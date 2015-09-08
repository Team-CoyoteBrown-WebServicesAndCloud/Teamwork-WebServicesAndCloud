namespace SocialNetwork.Services.Models.ViewModels.Comments
{
    using System;
    using User;

    public abstract class BaseCommentViewModel
    {
        public int Id { get; set; }

        public UserViewModelMinified Author { get; set; }

        public DateTime Date { get; set; }

        public int LikesCount { get; set; }

        public string CommentContent { get; set; }

        public bool Liked { get; set; }
    }
}