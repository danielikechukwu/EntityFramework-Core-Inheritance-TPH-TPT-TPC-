namespace EF_DatabaseInheritance.Entities
{
    public class WalletPayment : Payment
    {
        public string WalletType { get; set; } = null!;

        public decimal WalletBalanceUsed { get; set; }

        public decimal CashbackReceived { get; set; }
    }
}
