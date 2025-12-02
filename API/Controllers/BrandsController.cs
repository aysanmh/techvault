
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
        
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController(IGenericRepository<Brand> repository) : ControllerBase
    {
      
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Brand>>> GetBrands()
        {
            return Ok(await repository.ListAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Brand>> GetBrandById(int id)
        {
            var brand = await repository.GetByIdAsync(id);

            if(brand == null) return NotFound();

            return brand;
        }

        [HttpPost]
        public async Task<ActionResult<Brand>> CreateBrand(Brand brand)
        {
            repository.Add(brand);
            
            if(await repository.SaveAllAsync())
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
            
            repository.Update(brand);
            
            if(await repository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem updating the brand");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            var brand = await repository.GetByIdAsync(id);

            if(brand == null) return NotFound();

            repository.Remove(brand);

            if(await repository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem deleting the brand");
        }

        


        private bool BrandExists(int id)
        {
            return repository.Exists(id);
        }
    }

}