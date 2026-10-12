using System.ComponentModel.DataAnnotations;

namespace EF_DatabaseInheritance.DTOs
{
    public class NotificationCreateDTO
    {
        [Required]
        public string Message { get; set; } = null!;

        [Required]
        public int UserId { get; set; }

        [Required]
        [RegularExpression("Email|SMS|Push", ErrorMessage = "Invalid NotificationType")]
        public string NotificationType { get; set; } = null!;

        public string? Priority { get; set; }

        // Email
        public string? RecipientEmail { get; set; }

        public string? Subject { get; set; }

        public string? Cc { get; set; }

        public string? Bcc { get; set; }

        public bool? IsHtml { get; set; }

        // SMS
        public string? RecipientNumber { get; set; }

        public string? SenderId { get; set; }

        public string? Provider { get; set; }

        // Push
        public string? DeviceToken { get; set; }

        public string? AppPlatform { get; set; }

        public string? AppVersion { get; set; }
    }

    public class NotificationReadDTO
    {
        public int Id { get; set; }

        public string Message { get; set; } = null!;

        public string NotificationType { get; set; } = null!;

        public DateTime SentAt { get; set; }

        public bool IsDelivered { get; set; }

        public string? Priority { get; set; }
    }
}
