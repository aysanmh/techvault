using API.Extensions;
using API.SignalR;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Stripe;

namespace API.Controllers
{
    public class PaymentsController(
        IPaymentService paymentService,
        IUnitOfWork unit,
        ILogger<PaymentsController> logger,
        IConfiguration configuration,
        IHubContext<NotificationHub> hubContext
    ) : BaseApiController
    {
        private readonly string _webHookSecret = configuration["StripeSettings:WhSecret"]!;

        [Authorize]
        [HttpPost("{cartId}")]
        public async Task<ActionResult<ShoppingCart>> CreateOrUpdatePaymentIntent(string cartId)
        {
            var cart = await paymentService.CreateOrUpdatePaymentIntent(cartId);
            if (cart == null) return BadRequest("Problem with your cart");
            return Ok(cart);
        }

        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await unit.Repository<DeliveryMethod>().ListAllAsync());
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _webHookSecret);

                if (stripeEvent.Data.Object is not PaymentIntent intent)
                    return BadRequest("Invalid event data");

                var spec = new OrderSpecification(intent.Id, true);
                var order = await unit.Repository<Order>().GetEntityWithSpec(spec);

                if (order == null)
                {
                    logger.LogWarning("Order not found for PaymentIntent {IntentId}", intent.Id);
                    return Ok();
                }

                if (intent.Status == "succeeded")
                {
                    order.Status = ((long)order.GetTotal() * 100 != intent.Amount)
                        ? OrderStatus.PaymentMismatch
                        : OrderStatus.PaymentReceived;

                    await unit.Complete();

                    var connectionId = NotificationHub.GetConnectionIdByEmail(order.BuyerEmail);
                    if (!string.IsNullOrEmpty(connectionId))
                    {
                        await hubContext.Clients.Client(connectionId)
                            .SendAsync("OrderCompleteNotification", order.ToDto());
                    }
                }

                return Ok();
            }
            catch (StripeException ex)
            {
                logger.LogError(ex, "Stripe webhook error");
                return StatusCode(500, "Stripe webhook error");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error in Stripe webhook");
                return StatusCode(500, "Unexpected error");
            }
        }
    }
}
