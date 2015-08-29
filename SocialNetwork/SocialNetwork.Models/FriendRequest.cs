namespace SocialNetwork.Models
{
    using System.ComponentModel.DataAnnotations;
    using Enum;

    public class FriendRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FromUserId { get; set; }

        public virtual ApplicationUser FromUser { get; set; }

        [Required]
        public string ToUserId { get; set; }

        public virtual ApplicationUser ToUser { get; set; }

        public FriendRequestStatus FriendRequestStatus { get; set; }
    }
}