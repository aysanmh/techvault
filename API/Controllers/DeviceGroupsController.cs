
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeviceGroupsController(IDeviceGroupRepository repository) : ControllerBase
{
    
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DeviceGroup>>> GetDeviceGroup()
    {
        return Ok(await repository.GetDeviceGroupsAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DeviceGroup>> GetDeviceGroupById(int id)
    {
        var group = await repository.GetDeviceGroupByIdAsync(id);

        if(group == null) return NotFound();

        return group;
    }



    [HttpGet("{id:int}/devices")]
        public async Task<ActionResult<IReadOnlyList<Device>>> GetDevicesByGroup(int id)
        {
            var group = await repository.GetDeviceGroupByIdAsync(id);
            if (group == null) return NotFound();

            
            var devices = await repository.GetDevicesAsync(id);

            return Ok(devices);
        }

    [HttpPost]
    public async Task<ActionResult<DeviceGroup>> CreateDeviceGroup(DeviceGroup group)
    {
        repository.AddDeviceGroup(group);

        if(await repository.SaveChangesAsync())
        {
            return CreatedAtAction("GetDeviceGroupById",new{id = group.Id},group);
        }

        return BadRequest("Problem creating group");

    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateDeviceGroup(int id,DeviceGroup group)
    {
        if(group.Id != id || !GroupExists(id))
            return BadRequest("Cannot update group");

        repository.UpdateDeviceGroup(group);

        if(await repository.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem updating the group");

    }



    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteGroup(int id)
    {
        var group = await repository.GetDeviceGroupByIdAsync(id);

        if(group == null) return NotFound();

        repository.DeleteDeviceGroup(group);

         if(await repository.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting the group");
    }
    private bool GroupExists(int id)
    {
        return repository.DeviceGroupExists(id);
    }

    
}