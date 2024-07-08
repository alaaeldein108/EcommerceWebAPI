﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Service.BasketService.Dtos;
using Store.Service.OrderService.Dtos;
using Store.Service.PaymentService;
using Stripe;

namespace Store.API.Controllers
{
    
    public class PaymentController : BaseController
    {
        private readonly IPaymentService paymentService;
        private readonly ILogger<PaymentController> logger;
        private const string endpointSecret = "whsec_72ce80d3ef5d2df5a5296621141c89dd979c6fcf9734e2f0dc2902834a2396c1";

        public PaymentController(IPaymentService paymentService,
            ILogger<PaymentController> logger)
        {
            this.paymentService = paymentService;
            this.logger = logger;
        }
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForExistingOrder(CustomerBasketDto input)
        =>Ok(await paymentService.CreateOrUpdatePaymentIntentForExistingOrder(input));
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForNewOrder(string basketId)
        => Ok(await paymentService.CreateOrUpdatePaymentIntentForNewOrder(basketId));

       
        [HttpPost("webhook")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], endpointSecret);
            PaymentIntent paymentIntent;
            OrderResultDto order;
            // Handle the event
            if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
            {
                paymentIntent= (PaymentIntent)stripeEvent.Data.Object;
                logger.LogInformation("PaymentFailed : ",paymentIntent.Id);
                order = await paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
                logger.LogInformation("Order Updated To Payment Failed : ", order.Id);

            }
            else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            {
                paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                logger.LogInformation("Payment Succeed : ", paymentIntent.Id);
                order = await paymentService.UpdateOrderPaymentSuccessed(paymentIntent.Id);
                logger.LogInformation("Order Updated To Payment Succeed : ", order.Id);
            }
            // ... handle other event types
            else
            {
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }

            return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    
    }
}
