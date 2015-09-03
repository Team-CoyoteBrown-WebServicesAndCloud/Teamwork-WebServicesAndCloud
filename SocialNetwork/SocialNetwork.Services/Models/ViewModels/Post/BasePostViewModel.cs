namespace SocialNetwork.Services.Models.ViewModels.Post
{
    using System;

    public class BasePostViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }
      
        public DateTime PostedOn { get; set; }
       
        public string AuthorId { get; set; }

        public string WallOwnerId { get; set; }
    }
}