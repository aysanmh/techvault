
using API.DTOs;
using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class DevicesController(IUnitOfWork unit) : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<Pagination<DeviceDto>>> GetDevices([FromQuery] DeviceSpecParams specParams)
    {
        var spec = new DeviceSpecification(specParams);

        var devices = await unit.Repository<Device>().ListAsync(spec);
        var count = await unit.Repository<Device>().CountAsync(spec);

        var dtos = devices.Select(d => new DeviceDto
        {
            Id = d.Id,
            Model = d.Model,
            ImageUrl = d.ImageUrl,
            Description = d.Description,
            Price = d.Price,
            BrandName = d.Brand!.BrandName,
            DeviceGroupName = d.DeviceGroup!.GroupName
        }).ToList();

        return CreatePagedResult(dtos, specParams.PageIndex, specParams.PageSize, count);
    }



    [HttpGet("{id:int}")]
    public async Task<ActionResult<DeviceDto>> GetDeviceById(int id)
    {
        var spec = new DeviceByIdSpecification(id);
        var device = await unit.Repository<Device>().GetEntityWithSpec(spec);

        if (device == null)
            return NotFound(new { Message = $"Device with id {id} not found." });

        return new DeviceDto
        {
            Id = device.Id,
            Model = device.Model,
            ImageUrl = device.ImageUrl,
            Description = device.Description,
            Price = device.Price,
            BrandName = device.Brand!.BrandName,
            DeviceGroupName = device.DeviceGroup!.GroupName
        };
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<Brand>>> GetBrands()
    {
        var spec = new BrandListSpecification();
        var brands = await unit.Repository<Device>().ListAsync(spec);

        if (brands == null || !brands.Any())
            return NotFound(new { Message = "No brands found." });

        return Ok(brands);
    }

   [HttpGet("groups")]
    public async Task<ActionResult<IReadOnlyList<DeviceGroup>>> GetGroups()
    {
        var spec = new GroupListSpecification();
        var groups = await unit.Repository<Device>().ListAsync(spec);

        if (groups == null || !groups.Any())
            return NotFound(new { Message = "No device groups found." });

        return Ok(groups);
    }

    [Authorize(Roles ="Admin")] 
    [HttpPost]
    public async Task<ActionResult<DeviceDto>> CreateDevice(DeviceCreateDto dto)
    {
    
        try
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

            unit.Repository<Device>().Add(device);

            if (!await unit.Complete())
                return StatusCode(500, new { Message = "Problem creating device in the database." });

           
            var spec = new DeviceByIdSpecification(device.Id);
            var createdDevice = await unit.Repository<Device>().GetEntityWithSpec(spec);

            var deviceToReturn = new DeviceDto
            {
                Id = createdDevice!.Id,
                Model = createdDevice.Model,
                ImageUrl = createdDevice.ImageUrl,
                Description = createdDevice.Description,
                Price = createdDevice.Price,
                BrandName = createdDevice.Brand!.BrandName,
                DeviceGroupName = createdDevice.DeviceGroup!.GroupName
            };

            return CreatedAtAction(nameof(GetDeviceById),
                new { id = createdDevice.Id },
                deviceToReturn);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = $"Internal server error: {ex.Message}" });
        }
    }



    [Authorize(Roles ="Admin")] 
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateDevice(int id, DeviceCreateDto dto)
    {
        try
        {
            var spec = new DeviceByIdSpecification(id);
            var device = await unit.Repository<Device>().GetEntityWithSpec(spec);

            if (device == null)
                return NotFound(new { Message = $"Device with id {id} not found." });

            device.BrandId = dto.BrandId;
            device.DeviceGroupId = dto.DeviceGroupId;
            device.Model = dto.Model;
            device.ImageUrl = dto.ImageUrl;
            device.Description = dto.Description;
            device.Price = dto.Price;

            unit.Repository<Device>().Update(device);

            if (!await unit.Complete())
                return StatusCode(500, new { Message = "Problem updating device in the database." });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = $"Internal server error: {ex.Message}" });
        }
    }


    [Authorize(Roles ="Admin")] 
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteDevice(int id)
    {
        try
        {
            var spec = new DeviceByIdSpecification(id);
            var device = await unit.Repository<Device>().GetEntityWithSpec(spec);

            if (device == null)
                return NotFound(new { Message = $"Device with id {id} not found." });

            unit.Repository<Device>().Remove(device);

            if (!await unit.Complete())
                return StatusCode(500, new { Message = "Problem deleting device from the database." });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = $"Internal server error: {ex.Message}" });
        }
    }

      private bool DeviceExists(int id) => unit.Repository<Device>().Exists(id);
   

}


