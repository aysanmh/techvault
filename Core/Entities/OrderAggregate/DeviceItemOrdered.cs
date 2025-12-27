
namespace Core.Entities.OrderAggregate
{
    public class DeviceItemOrdered
    {
        public int DeviceId { get; set; }

        public required string DeviceName { get; set; }

        public required string ImageUrl { get; set; }
        
    }
}