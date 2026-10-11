namespace EF_DatabaseInheritance.Entities
{
    public class Seller : User
    {
        public string BusinessName { get; set; } = null!;

        public string GSTNumber { get; set; } = null!;

        public string PANNumber { get; set; } = null!;

        public string WarehouseAddress { get; set; } = null!;

        public string? BusinessLicenseNo { get; set; }

        public string? BankAccountNumber { get; set; }

        public bool IsVerified { get; set; } = false;

        public string? SupportPhone { get; set; }
    }
}
