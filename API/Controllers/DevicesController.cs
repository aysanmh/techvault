
using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController(IDeviceRepository repository) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DeviceDto>>> GetDevices()
    {
        var devices = await repository.GetDevicesAsync();

        var dto = devices.Select(d => new DeviceDto
        {
            Id = d.Id,
            Model = d.Model,
            ImageUrl = d.ImageUrl,
            Description = d.Description,
            Price = d.Price,
            BrandName = d.Brand?.BrandName,
            DeviceGroupName = d.Group?.GroupName

        }).ToList();
            
        return Ok(dto);
        
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DeviceDto>> GetDeviceById(int id)
    {
        var device = await repository.GetDeviceByIdAsync(id);

        if (device == null) return NotFound();

        return new DeviceDto
        {
            Id = device.Id,
            Model = device.Model,
            ImageUrl = device.ImageUrl,
            Description = device.Description,
            Price = device.Price,
            BrandName = device.Brand?.BrandName,
            DeviceGroupName = device.Group?.GroupName
        };
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

        repository.AddDevice(device);

        if (await repository.SaveChangesAsync())
        {
            var deviceToReturn = new DeviceDto
            {
                Id = device.Id,
                Model = device.Model,
                ImageUrl = device.ImageUrl,
                Description = device.Description,
                Price = device.Price,
                BrandName = device.Brand?.BrandName, 
                DeviceGroupName = device.Group?.GroupName
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
        
        var device = await repository.GetDeviceByIdAsync(id);

        if (device == null) return NotFound();


        device.BrandId = dto.BrandId;

        device.DeviceGroupId = dto.DeviceGroupId;

        device.Model = dto.Model;

        device.ImageUrl = dto.ImageUrl;

        device.Description = dto.Description;

        device.Price = dto.Price;


        repository.UpdateDevice(device);

        if (await repository.SaveChangesAsync())
            return NoContent();

        return BadRequest("Problem updating device");
    }


     [HttpDelete("{id:int}")]
     public async Task<ActionResult> DeleteDevice(int id)
    {
        var device = await repository.GetDeviceByIdAsync(id);


        if (device == null) return NotFound();

        repository.DeleteDevice(device);

        if (await repository.SaveChangesAsync())
            return NoContent();

        return BadRequest("Problem deleting device");
    }
   

}


