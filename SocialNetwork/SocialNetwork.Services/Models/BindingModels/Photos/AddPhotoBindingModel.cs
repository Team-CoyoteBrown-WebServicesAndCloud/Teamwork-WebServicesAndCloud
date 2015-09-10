namespace SocialNetwork.Services.Models.BindingModels.Photos
{
    using System.ComponentModel.DataAnnotations;

    public class AddPhotoBindingModel
    {
        [Required]
        public string Image { get; set; }

        public string Description { get; set; }
    }
}