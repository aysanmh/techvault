
using API.DTOs;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{

    private readonly StoreContext context;
    public DevicesController(StoreContext context)
    {
        this.context = context;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeviceDto>>> GetDevices()
    {
        var devices = await context.Devices
            .Include(d => d.Brand)
            .Include(d => d.Group)
            .Select(d => new DeviceDto
            {
                Id = d.Id,
                Model = d.Model,
                ImageUrl = d.ImageUrl,
                Description = d.Description,
                Price = d.Price,
                BrandName = d.Brand!.BrandName,
                DeviceGroupName = d.Group!.GroupName
            })
            .ToListAsync();

        return Ok(devices);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DeviceDto>> GetDeviceById(int id)
    {
        var device = await context.Devices
            .Include(d => d.Brand)
            .Include(d => d.Group)
            .Where(d => d.Id == id)
            .Select(d => new DeviceDto
            {
                Id = d.Id,
                Model = d.Model,
                ImageUrl = d.ImageUrl,
                Description = d.Description,
                Price = d.Price,
                BrandName = d.Brand!.BrandName,
                DeviceGroupName = d.Group!.GroupName
            })
            .FirstOrDefaultAsync();

        if (device == null) return NotFound();

        return Ok(device);
    }

    [HttpPost]
    public async Task<ActionResult<DeviceDto>> CreateDevice(DeviceCreateDto deviceDto)
    {
        var device = new Device
        {
            BrandId = deviceDto.BrandId,
            DeviceGroupId = deviceDto.DeviceGroupId,
            Model = deviceDto.Model,
            ImageUrl = deviceDto.ImageUrl,
            Description = deviceDto.Description,
            Price = deviceDto.Price
        };

        context.Devices.Add(device);
        await context.SaveChangesAsync();

        var deviceToReturn = await context.Devices
            .Include(d => d.Brand)
            .Include(d => d.Group)
            .Where(d => d.Id == device.Id)
            .Select(d => new DeviceDto
            {
                Id = d.Id,
                Model = d.Model,
                ImageUrl = d.ImageUrl,
                Description = d.Description,
                Price = d.Price,
                BrandName = d.Brand!.BrandName,
                DeviceGroupName = d.Group!.GroupName
            })
            .FirstOrDefaultAsync();

        return CreatedAtAction(nameof(GetDeviceById), new { id = device.Id }, deviceToReturn);
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateDevice(int id, DeviceCreateDto dto)
    {
        var device = await context.Devices.FindAsync(id);

        if (device == null)
            return NotFound();

        // Apply updates
        device.BrandId = dto.BrandId;
        device.DeviceGroupId = dto.DeviceGroupId;
        device.Model = dto.Model;
        device.ImageUrl = dto.ImageUrl;
        device.Description = dto.Description;
        device.Price = dto.Price;

        await context.SaveChangesAsync();

        return NoContent();
    }


     [HttpDelete("{id:int}")]
     public async Task<ActionResult> DeleteDevice(int id)
    {
        var device = await context.Devices.FindAsync(id);

        if(device == null) return NotFound();

        context.Devices.Remove(device);

        await context.SaveChangesAsync();

        return NoContent();
    }
    private bool DeviceExists(int id)
    {
        return context.Devices.Any(x => x.Id == id);
    }


}


