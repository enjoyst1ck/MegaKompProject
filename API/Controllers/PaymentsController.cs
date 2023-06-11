using API.Errors;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace API.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private const string WhSecret = "whsec_3df0b68ac2cb9bdbe07095c7150bb1d6b4328c6bdc814a72b95b456fe34cbfb8";
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket == null) return BadRequest(new ApiResponse(400, "Problem z koszykiem"));
            
            return basket;
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json, 
                Request.Headers["Stripe-Signature"], WhSecret);

            PaymentIntent intent;
            Order order;

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    intent = (PaymentIntent) stripeEvent.Data.Object;
                    _logger.LogInformation("Płatność powiodła się: ", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentSucceeded(intent.Id);
                    _logger.LogInformation("Zamówienie zaaktualizowane, płatność odebrana", order.Id);
                    break;

                case "payment_intent.payment_failed":
                    intent = (PaymentIntent) stripeEvent.Data.Object;
                    _logger.LogInformation("Płatność nie powiodła się: ", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentFailed(intent.Id);
                    _logger.LogInformation("Zamówienie zaaktualizowane, płatność nie powiodła sie", order.Id);
                    break;
            }
            
            return new EmptyResult();
        }

    }
}