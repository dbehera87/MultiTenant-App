using Microsoft.AspNetCore.Mvc;
using Payment.Models;
using Payment.Services;

namespace Payment.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        /// <summary>
        /// Make a payment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index([FromBody]PaymentRequest request)
        {
            var isSuccess = await paymentService.MakePaymentAsync(request);
            return isSuccess ? Ok(): BadRequest();
        }
    }
}
