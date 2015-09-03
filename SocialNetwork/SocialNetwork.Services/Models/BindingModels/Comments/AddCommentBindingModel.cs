namespace SocialNetwork.Services.Models.BindingModels.Comments
{
    using ViewModels.User;

    public class AddCommentBindingModel
    {
        public int Id { get; set; }

        public string CommentContent { get; set; }

        public UserViewModelMinified Author { get; set; }
    }
}