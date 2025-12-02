
using Core.Entities;

namespace Core.Interfaces
{
    public interface IBrandRepository
    {

        Task<IReadOnlyList<Brand>> GetBrandsAsync();

        Task<Brand?> GetBrandByIdAsync(int id);

        

        void AddBrand(Brand brand);

        void UpdateBrand(Brand brand);

        void DeleteBrand(Brand brand);

        bool BrandExists(int id);

        Task<bool> SaveChangesAsync();
        
    }
}