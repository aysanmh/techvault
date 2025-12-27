
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
        
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController(IUnitOfWork unit) : ControllerBase
    {
      
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Brand>>> GetBrands()
        {
            return Ok(await unit.Repository<Brand>().ListAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Brand>> GetBrandById(int id)
        {
            var brand = await unit.Repository<Brand>().GetByIdAsync(id);

            if(brand == null) return NotFound();

            return brand;
        }

        [HttpPost]
        public async Task<ActionResult<Brand>> CreateBrand(Brand brand)
        {
            unit.Repository<Brand>().Add(brand);
            
            if(await unit.Complete())
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
            
            unit.Repository<Brand>().Update(brand);
            
            if(await unit.Complete())
            {
                return NoContent();
            }

            return BadRequest("Problem updating the brand");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            var brand = await unit.Repository<Brand>().GetByIdAsync(id);

            if(brand == null) return NotFound();

            unit.Repository<Brand>().Remove(brand);

            if(await unit.Complete())
            {
                return NoContent();
            }

            return BadRequest("Problem deleting the brand");
        }

        


        private bool BrandExists(int id)
        {
            return unit.Repository<Brand>().Exists(id);
        }
    }

}