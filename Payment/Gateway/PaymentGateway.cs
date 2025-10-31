using Newtonsoft.Json;
using Payment.Models;

namespace Payment.Gateway
{
    public partial class PaymentGateway: IPaymentGateway
    {
        private readonly HttpClient httpClient;

        public PaymentGateway(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> ProcessPaymentAsync(PaymentRequest request)
        {
            var response = await httpClient.PostAsJsonAsync("/api/external/pay", JsonConvert.SerializeObject(request));

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PaymentResult>();
            return result?.Success ?? false;
        }
    }
}
