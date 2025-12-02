
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class BrandRepository(StoreContext context) : IBrandRepository
    {
        public void AddBrand(Brand brand)
        {
            context.Brands.Add(brand);
        }

        public void DeleteBrand(Brand brand)
        {
            context.Brands.Remove(brand);
        }

        public bool BrandExists(int id)
        {
            return context.Brands.Any(x => x.Id == id);
        }

        
        public async Task<Brand?> GetBrandByIdAsync(int id)
        {
            return await context.Brands.FindAsync(id);
        }

        public async Task<IReadOnlyList<Brand>> GetBrandsAsync()
        {
           return await context.Brands.ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void UpdateBrand(Brand brand)
        {
            context.Entry(brand).State = EntityState.Modified;
        }

       
    }
}