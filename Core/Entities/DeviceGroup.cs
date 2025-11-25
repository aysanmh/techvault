
namespace Core.Entities
{
    public class DeviceGroup : BaseEntity
    {

        public required string GroupName { get; set; }

        public ICollection<Device> Devices { get; set; } = new List<Device>();
     
    }
}