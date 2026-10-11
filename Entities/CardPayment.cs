namespace EF_DatabaseInheritance.Entities
{
    public class CardPayment : Payment
    {
        public string CardNumber { get; set; } = null!;

        public string CardHolderName { get; set; } = null!;

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public string CVV { get; set; } = null!;

        public string MaskedCard => $"XXXX-XXXX-XXXX-{CardNumber[^4..]}";
    }
}
