
using Core.Entities;

namespace Core.Specification
{
    public class DeviceSpecification : BaseSpecification<Device>
    {
        public DeviceSpecification(string? brand,string? deviceGroup, string? sort) : base(x =>
        (string.IsNullOrWhiteSpace(brand) || x.Brand.BrandName == brand) &&
        (string.IsNullOrWhiteSpace(deviceGroup) || x.DeviceGroup.GroupName== deviceGroup)
        )
        {
            AddInclude(x => x.Brand);
            AddInclude(x => x.DeviceGroup);

            switch(sort)
            {
                case "priceAsc":
                    AddOrderBy(x => x.Price);
                    break;
                case "priceDesc": 
                    AddOrderByDescending(x => x.Price);
                    break;
                default:
                    AddOrderBy(x => x.Id);
                    break;  
            }
            
        }
    }
}