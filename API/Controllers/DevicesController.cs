
using API.DTOs;
using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class DevicesController(IGenericRepository<Device> repository) : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<Pagination<DeviceDto>>> GetDevices([FromQuery] DeviceSpecParams specParams)
    {
        var spec = new DeviceSpecification(specParams);

        var devices = await repository.ListAsync(spec);
        var count = await repository.CountAsync(spec);

        var dtos = devices.Select(d => new DeviceDto
        {
            Id = d.Id,
            Model = d.Model,
            ImageUrl = d.ImageUrl,
            Description = d.Description,
            Price = d.Price,
            BrandName = d.Brand?.BrandName,
            DeviceGroupName = d.DeviceGroup?.GroupName
        }).ToList();

        return CreatePagedResult(dtos, specParams.PageIndex, specParams.PageSize, count);
    }



    [HttpGet("{id:int}")]
    public async Task<ActionResult<DeviceDto>> GetDeviceById(int id)
    {
        var device = await repository.GetByIdAsync(id);

        if (device == null) return NotFound();

        return new DeviceDto
        {
            Id = device.Id,
            Model = device.Model,
            ImageUrl = device.ImageUrl,
            Description = device.Description,
            Price = device.Price,
            BrandName = device.Brand?.BrandName,
            DeviceGroupName = device.DeviceGroup?.GroupName
        };
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<Brand>>> GetBrands()
    {
        var spec = new BrandListSpecification();

        return Ok(await repository.ListAsync(spec));
    }


    [HttpGet("groups")]
    public async Task<ActionResult<IReadOnlyList<DeviceGroup>>> GetGroups()
    {

        var spec = new GroupListSpecification();

        return Ok(await repository.ListAsync(spec));
    }
     
    [HttpPost]
    public async Task<ActionResult<DeviceDto>> CreateDevice(DeviceCreateDto dto)
    {
        var device = new Device
        {
            BrandId = dto.BrandId,
            DeviceGroupId = dto.DeviceGroupId,
            Model = dto.Model,
            ImageUrl = dto.ImageUrl,
            Description = dto.Description,
            Price = dto.Price
        };

        repository.Add(device);

        if (await repository.SaveAllAsync())
        {
            var deviceToReturn = new DeviceDto
            {
                Id = device.Id,
                Model = device.Model,
                ImageUrl = device.ImageUrl,
                Description = device.Description,
                Price = device.Price,
                BrandName = device.Brand?.BrandName, 
                DeviceGroupName = device.DeviceGroup?.GroupName
            };

            return CreatedAtAction(nameof(GetDeviceById),
                new { id = device.Id },
                deviceToReturn);
        }

        return BadRequest("Problem creating device");
    }



    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateDevice(int id, DeviceCreateDto dto)
    {   
        
        var device = await repository.GetByIdAsync(id);

        if (device == null) return NotFound();


        device.BrandId = dto.BrandId;

        device.DeviceGroupId = dto.DeviceGroupId;

        device.Model = dto.Model;

        device.ImageUrl = dto.ImageUrl;

        device.Description = dto.Description;

        device.Price = dto.Price;


        repository.Update(device);

        if (await repository.SaveAllAsync())
            return NoContent();

        return BadRequest("Problem updating device");
    }


     [HttpDelete("{id:int}")]
     public async Task<ActionResult> DeleteDevice(int id)
    {
        var device = await repository.GetByIdAsync(id);


        if (device == null) return NotFound();

        repository.Remove(device);

        if (await repository.SaveAllAsync())
            return NoContent();

        return BadRequest("Problem deleting device");
    }

    private bool DevcieExists(int id)
    {
        return repository.Exists(id);
    }
   

}


