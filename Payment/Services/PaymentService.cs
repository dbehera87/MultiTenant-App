using Payment.Gateway;
using Payment.Models;

namespace Payment.Services
{
    public partial class PaymentService: IPaymentService
    {
        private readonly IPaymentGateway paymentGateway;

        public PaymentService(IPaymentGateway paymentGateway)
        {
            this.paymentGateway = paymentGateway;
        }

        public async Task<bool> MakePaymentAsync(PaymentRequest request)
        {
            return await paymentGateway.ProcessPaymentAsync(request);
        }
    }
}
