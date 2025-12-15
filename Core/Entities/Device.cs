
namespace Core.Entities
{
    public class Device : BaseEntity
    {

        public int DeviceGroupId { get; set; }
        public  DeviceGroup? DeviceGroup { get; set; }

        public int BrandId { get; set; }
        public   Brand? Brand { get; set; }

        public required string Model{ get; set; }

        public required string ImageUrl { get; set; }

        public required string Description { get; set; }
        
        
        public decimal Price { get; set; }

        


        
    }
}