namespace SocialNetwork.Models
{
    using Enum;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public DateTime NotificationOn { get; set; }

        public NotificationType NotificationType { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        public virtual ApplicationUser Receiver { get; set; }
    }
}