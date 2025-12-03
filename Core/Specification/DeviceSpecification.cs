
using Core.Entities;

namespace Core.Specification
{
    public class DeviceSpecification : BaseSpecification<Device>
    {
        public DeviceSpecification(DeviceSpecParams specParams) : base(x =>
        (string.IsNullOrEmpty(specParams.Search) || x.Model.ToLower().Contains(specParams.Search)) &&
        (specParams.Brands.Count == 0 || specParams.Brands.Contains(x.Brand.BrandName)) &&
        (specParams.DeviceGroups.Count == 0 || specParams.DeviceGroups.Contains(x.DeviceGroup.GroupName))
        )
        {
            AddInclude(x => x.Brand);
            AddInclude(x => x.DeviceGroup);

            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
            
             switch(specParams.Sort)    
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