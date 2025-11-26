
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
        
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController(IBrandRepository repository) : ControllerBase
    {
      
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Brand>>> GetBrands()
        {
            return Ok(await repository.GetBrandsAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Brand>> GetBrandById(int id)
        {
            var brand = await repository.GetBrandByIdAsync(id);

            if(brand == null) return NotFound();

            return brand;
        }

        [HttpPost]
        public async Task<ActionResult<Brand>> CreateBrand(Brand brand)
        {
            repository.AddBrand(brand);
            
            if(await repository.SaveChangesAsync())
            {
                return CreatedAtAction("GetBrandById",new{id = brand.Id},brand);
            }

            return BadRequest("Problem creating brand");
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateBrand(int id,Brand brand)
        {
            if(brand.Id != id || !BrandExists(id))
                return BadRequest("Cannot update brand");
            
            repository.UpdateBrand(brand);
            
            if(await repository.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem updating the brand");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            var brand = await repository.GetBrandByIdAsync(id);

            if(brand == null) return NotFound();

            repository.DeleteBrand(brand);

            if(await repository.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem deleting the brand");
        }

        [HttpGet("{id:int}/devices")]
        public async Task<ActionResult<IReadOnlyList<Device>>> GetDevicesByBrand(int id)
        {
            var brand = await repository.GetBrandByIdAsync(id);
            if (brand == null) return NotFound();

            
            var devices = await repository.GetDevicesByBrandAsync(id);

            return Ok(devices);
        }


        private bool BrandExists(int id)
        {
            return repository.BrandExists(id);
        }
    }

}