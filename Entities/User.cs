namespace EF_DatabaseInheritance.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? CreatedBy { get; set; } = "System";
    }
}
