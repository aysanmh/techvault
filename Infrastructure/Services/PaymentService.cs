using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
         private readonly ICartService cartService;
         private readonly IUnitOfWork unit;
        public PaymentService(IConfiguration configuration, ICartService cartService, 
                IUnitOfWork unit)
        {
            this.cartService = cartService;
            
            this.unit = unit;

            StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];

        }
        
        public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
        {
            var cart = await cartService.GetCartAsync(cartId)
                ?? throw new Exception("Cart unavailable");
            
            var shippingPrice = await GetShippingPriceAsync(cart) ?? 0 ;

            await ValidateCartItemsInCartAsync(cart);

            var subtotal = CalculateSubtotal(cart);

            var total = subtotal + shippingPrice;

            await CreateUpdatePaymentIntentAsync(cart,total);

            await cartService.SetCartAsync(cart);

            return cart;

        }



        public async Task<string> RefundPayment(string paymentIntentId)
        {
            var refundOptions = new RefundCreateOptions
            {
                PaymentIntent = paymentIntentId
            };
            var refundService = new RefundService();
            var result = await refundService.CreateAsync(refundOptions);
            return result.Status;
        }

        private long CalculateSubtotal(ShoppingCart cart)
        {
            var itemTotal = cart.Items.Sum(x => x.Quantity * x.Price * 100);
            return (long)itemTotal;
        }


        private async Task CreateUpdatePaymentIntentAsync(ShoppingCart cart, long total)
        {
            var service = new PaymentIntentService();

            if (string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                  Amount = total,
                  Currency = "usd",
                  PaymentMethodTypes = ["card"]  
                };
                var intent =  await service.CreateAsync(options);
                cart.PaymentIntentId = intent.Id;
                cart.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                  Amount = total  
                };
                await service.UpdateAsync(cart.PaymentIntentId,options);
            }
        }

        private async Task ValidateCartItemsInCartAsync(ShoppingCart cart)
        {
            foreach (var item in cart.Items)
            {
                var deviceItem = await unit.Repository<Device>()
                    .GetByIdAsync(item.DeviceId) ?? throw new Exception("Problem getting device.");

                if(item.Price != deviceItem.Price)
                {
                    item.Price = deviceItem.Price;
                }
                
            }
        }
        private async Task<long?> GetShippingPriceAsync(ShoppingCart cart)
        {
            if (cart.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await unit.Repository<DeliveryMethod>()
                    .GetByIdAsync((int)cart.DeliveryMethodId) ??
                        throw new Exception("Problem with delivery method.");
                return (long)deliveryMethod.Price * 100;
            }

            return null;
        }



    }

}