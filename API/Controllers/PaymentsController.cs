using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace API.Controllers
{
    public class PaymentsController : BaseAPIController
    {
        private readonly string _whSecret;
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(IPaymentService paymentService, IMapper mapper, ILogger<PaymentsController> logger, IConfiguration config)
        {
            _mapper = mapper;
            _logger = logger;
            _paymentService = paymentService;
            _whSecret = config.GetSection("StripeSettings:WhSecret").Value;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basketDto = _mapper.Map<CustomerBasketDto>(await _paymentService.CreateOrUpdatePaymentIntent(basketId));

            if(basketDto == null) return BadRequest(new ApiResponse(400, "Problem with your basket"));

            return basketDto;
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebHook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(
              json,
              Request.Headers["Stripe-Signature"],
              _whSecret,
              300,
              (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds,
              false);

            PaymentIntent intent;
            Order order;

            switch(stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    intent = (PaymentIntent) stripeEvent.Data.Object;
                    _logger.LogInformation("Payment succeeded: ", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentSucceeded(intent.Id);
                    _logger.LogInformation("Order updated to payment received: ", order.Id);
                    break;
                case "payment_intent.payment_failed":
                    intent = (PaymentIntent) stripeEvent.Data.Object;
                    _logger.LogInformation("Payment failed: ", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentFailed(intent.Id);
                    _logger.LogInformation("Order updated to payment failed: ", order.Id);
                    break;
            }

            return new EmptyResult();
        }
    }
}