namespace EF_DatabaseInheritance.Entities
{
    public class PushNotification : Notification
    {
        public string DeviceToken { get; set; } = null!;

        public string AppPlatform { get; set; } = null!;

        public string? AppVersion { get; set; }
    }
}
