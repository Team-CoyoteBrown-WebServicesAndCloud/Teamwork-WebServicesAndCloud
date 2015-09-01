namespace SocialNetwork.Services.Models.ViewModels.User
{
    using SocialNetwork.Models.Enum;

    public class BaseUserViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public Gender Gender { get; set; }

        public string ProfileImageData { get; set; }
    }
}