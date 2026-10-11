namespace EF_DatabaseInheritance.Entities
{
    public class Notification
    {
        public int Id { get; set; }

        public string Message { get; set; } = null!;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool IsDelivered { get; set; }

        public int UserId { get; set; }

        public string Priority { get; set; } = "Normal";

        public string? TemplateId { get; set; }

        public string? SentBy { get; set; }

        public int RetryCount { get; set; } = 0;

        public string? FailureReason { get; set; }

        public string? ResponseCode { get; set; }
    }
}
