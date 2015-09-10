namespace SocialNetwork.Services.Models.ViewModels.Groups
{
    public abstract class BaseGroupViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}