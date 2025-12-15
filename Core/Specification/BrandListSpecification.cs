
using Core.Entities;

namespace Core.Specification
{
    public class BrandListSpecification : BaseSpecification<Device,string>
    {
        public BrandListSpecification()
        {
            AddSelect(x => x.Brand!.BrandName);
            ApplyDistinct();
            
        }
    }
}