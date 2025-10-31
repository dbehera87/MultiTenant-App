using Payment.Models;

namespace Payment.Services
{
    public interface IPaymentService
    {
        Task<bool> MakePaymentAsync(PaymentRequest request);
    }
}
