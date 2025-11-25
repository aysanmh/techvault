
namespace Core.Entities
{
    public class Brand : BaseEntity
    {
        public required string BrandName { get; set; }

        public ICollection<Device> Devices { get; set; } = new List<Device>();
    }
}