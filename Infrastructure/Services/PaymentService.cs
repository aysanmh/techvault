using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class PaymentService(
        ICartService cartService,
        IGenericRepository<Device> deviceRepository,
        IGenericRepository<DeliveryMethod> deliveryRepository
    ) : IPaymentService
    {
        private readonly ICartService _cartService = cartService;
        private readonly IGenericRepository<Device> _deviceRepository = deviceRepository;
        private readonly IGenericRepository<DeliveryMethod> _deliveryRepository = deliveryRepository;

        public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
        {
            var cart = await _cartService.GetCartAsync(cartId);
            if (cart == null) return null;

            decimal shippingPrice = 0m;
            if (cart.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _deliveryRepository.GetByIdAsync(cart.DeliveryMethodId.Value);
                if (deliveryMethod == null) return null;
                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in cart.Items)
            {
                var deviceItem = await _deviceRepository.GetByIdAsync(item.DeviceId);
                if (deviceItem == null) return null;
                if (item.Price != deviceItem.Price)
                {
                    item.Price = deviceItem.Price;
                }
            }

            if (string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                cart.PaymentIntentId = "FAKE_" + Guid.NewGuid();
                cart.ClientSecret = "dummy_secret";
            }

            await _cartService.SetCartAsync(cart);
            return cart;
        }
    }
}
