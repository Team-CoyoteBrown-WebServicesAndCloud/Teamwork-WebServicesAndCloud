using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialNetwork.Services.Models.BindingModels.Post
{
    public class DeletePostBindingModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public string WallOwnerId { get; set; }
    }
}