using SocialNetwork.Models.Enum;
using System.ComponentModel.DataAnnotations;
namespace SocialNetwork.Models
{
    public class FriendRequest
    {
        public int Id { get; set; }

        [Required]
        public ApplicationUser FromUserId { get; set; }

        public virtual ApplicationUser FromUser { get; set; }

        [Required]
        public ApplicationUser ToUserId { get; set; }

        public virtual ApplicationUser ToUser { get; set; }

        public FriendRequestStatus FriendRequestStatus { get; set; }
    }
}