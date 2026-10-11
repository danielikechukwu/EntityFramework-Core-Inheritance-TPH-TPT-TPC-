namespace EF_DatabaseInheritance.Entities
{
    public class AdminUser : User
    {
        public string RoleName { get; set; } = null!;

        public string Department { get; set; } = null!;

        public string? Permissions { get; set; }

        public string? LoginIpAddress { get; set; }
    }
}
