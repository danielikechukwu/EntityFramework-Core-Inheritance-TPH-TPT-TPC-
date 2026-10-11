namespace EF_DatabaseInheritance.Entities
{
    public class SmsNotification : Notification
    {
        public string RecipientNumber { get; set; } = null!;

        public string SenderId { get; set; } = null!;

        public string Provider { get; set; } = null!;
    }
}
