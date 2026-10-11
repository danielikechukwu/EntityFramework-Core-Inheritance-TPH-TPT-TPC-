namespace EF_DatabaseInheritance.Entities
{
    public class UPIPayment
    {
        public string UpiId { get; set; } = null!;

        public string? AppName { get; set; }

        public string? TransactionRefNo { get; set; }
    }
}
