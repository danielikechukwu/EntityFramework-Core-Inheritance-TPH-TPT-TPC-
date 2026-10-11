using EF_DatabaseInheritance.Enums;

namespace EF_DatabaseInheritance.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = null!;

        public string PaymentGateway { get; set; } = null!;

        public int OrderId { get; set; }

        public string? TransactionId { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public string? Remarks { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
