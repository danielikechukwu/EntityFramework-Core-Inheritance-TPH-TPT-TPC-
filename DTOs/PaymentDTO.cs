using System.ComponentModel.DataAnnotations;

namespace EF_DatabaseInheritance.DTOs
{
    public class PaymentCreateDTO
    {
        // ---------- Common Fields ----------
        [Required(ErrorMessage = "Amount is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Currency is required.")]
        [StringLength(10)]
        public string Currency { get; set; } = null!;

        [Required(ErrorMessage = "PaymentGateway is required.")]
        [StringLength(50)]
        public string PaymentGateway { get; set; } = null!;

        [Required(ErrorMessage = "OrderId is required.")]
        public int OrderId { get; set; }

        [StringLength(100)]
        public string TransactionId { get; set; } = null!;

        [Required(ErrorMessage = "PaymentType is required.")]
        [StringLength(20)]
        public string PaymentType { get; set; } = null!;

        // ---------- Card-Specific ----------
        public string? CardNumber { get; set; }

        public string? CardHolderName { get; set; }

        public int? ExpiryMonth { get; set; }

        public int? ExpiryYear { get; set; }

        public string? CVV { get; set; }

        // ---------- UPI-Specific ----------
        public string? UpiId { get; set; }

        public string? AppName { get; set; }

        public string? TransactionRefNo { get; set; }

        // ---------- Wallet-Specific ----------
        public string? WalletType { get; set; }

        public decimal? WalletBalanceUsed { get; set; }

        public decimal? CashbackReceived { get; set; }
    }

    // ---------- DTO for Reading ----------
    public class PaymentReadDTO
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = null!;

        public string PaymentGateway { get; set; } = null!;

        public string PaymentStatus { get; set; } = null!;

        public DateTime PaymentDate { get; set; }

        public string PaymentType { get; set; } = null!;
    }
}
