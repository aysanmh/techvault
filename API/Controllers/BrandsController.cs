
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
        
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly StoreContext context;

        public BrandsController(StoreContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            return await context.Brands.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Brand>> GetBrandById(int id)
        {
            var brand = await context.Brands.FindAsync(id);

            if(brand == null) return NotFound();

            return brand;
        }

        [HttpPost]
        public async Task<ActionResult<Brand>> CreateBrand(Brand brand)
        {
            context.Brands.Add(brand);

            await context.SaveChangesAsync();

            return brand;
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateBrand(int id,Brand brand)
        {
            if(brand.Id != id || !BrandExists(id))
                return BadRequest("Cannot update brand");
            
            context.Entry(brand).State = EntityState.Modified;
            
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            var brand = await context.Brands.FindAsync(id);

            if(brand == null) return NotFound();

            context.Brands.Remove(brand);
            
            await context.SaveChangesAsync();

            return NoContent();

        }

        private bool BrandExists(int id)
        {
            return context.Brands.Any(x => x.Id == id);
        }
    }

}