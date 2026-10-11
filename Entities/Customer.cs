namespace EF_DatabaseInheritance.Entities
{
    public class Customer : User
    {
        public string? PhoneNumber { get; set; }

        public string? ShippingAddress { get; set; }

        public string? BillingAddress { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int LoyaltyPoints { get; set; } = 0;
    }
}
