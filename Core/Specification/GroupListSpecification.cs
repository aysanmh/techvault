
using Core.Entities;

namespace Core.Specification
{
    public class GroupListSpecification :BaseSpecification<Device,string>
    {
        public GroupListSpecification()
        {
            AddSelect(x => x.DeviceGroup.GroupName);
            ApplyDistinct();
        }
        
    }
}