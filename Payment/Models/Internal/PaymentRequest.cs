namespace Payment.Models
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string UserId { get; set; }
    }
}
