
namespace Core.Entities
{
    public class CartItem
    {

        public int DeviceId { get; set; }

        public required string DeviceName { get; set; }

        public decimal Price { get; set; }

        public required string ImageUrl { get; set; }

        public int Quantity { get; set; }

        
  
    }
}