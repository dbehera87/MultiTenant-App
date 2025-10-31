using Payment.Models;

namespace Payment.Gateway
{
    public interface IPaymentGateway
    {
        Task<bool> ProcessPaymentAsync(PaymentRequest request);
    }
}
