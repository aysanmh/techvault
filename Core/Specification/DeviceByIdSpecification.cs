using Core.Entities;

namespace Core.Specification
{
    public class DeviceByIdSpecification : BaseSpecification<Device>
    {
        public DeviceByIdSpecification(int id) : base(d => d.Id == id)
        {
            AddInclude(d => d.Brand!);
            AddInclude(d => d.DeviceGroup!);
        }
    }
}
