namespace EF_DatabaseInheritance.Entities
{
    public class EmailNotification : Notification
    {
        public string RecipientEmail { get; set; } = null!;

        public string Subject { get; set; } = null!;

        public string? Cc { get; set; }

        public string? Bcc { get; set; }

        public bool IsHtml { get; set; }
    }
}
