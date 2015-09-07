namespace SocialNetwork.Services.Models.ViewModels.Comments
{
    using System;
    using SocialNetwork.Models;
    using User;

    public class CommentViewModel
    {
        public int Id { get; set; }

        public UserViewModelMinified Author { get; set; }

        public DateTime Date { get; set; }

        public string CommentContent { get; set; }

        public static CommentViewModel Create(Comment comment, ApplicationUser currentUser)
        {
            return new CommentViewModel()
            {
                Id = comment.Id,
                Author = UserViewModelMinified.ConvertTo(comment.Author),
                Date = comment.PostedOn,
                CommentContent = comment.Content
            };
        }
    }
}