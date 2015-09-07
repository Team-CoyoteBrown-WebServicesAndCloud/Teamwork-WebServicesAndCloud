namespace SocialNetwork.Services.Models.BindingModels.Comments
{
    using System.ComponentModel.DataAnnotations;
    using ViewModels.User;

    public class AddCommentBindingModel
    {
        [Required]
        [MinLength(1, ErrorMessage = "The {0} must be at least {1} characters long")]
        public string CommentContent { get; set; }
    }
}