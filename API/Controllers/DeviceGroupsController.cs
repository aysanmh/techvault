
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeviceGroupsController(IUnitOfWork unit) : ControllerBase
{
    
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DeviceGroup>>> GetDeviceGroup()
    {
        return Ok(await unit.Repository<DeviceGroup>().ListAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DeviceGroup>> GetDeviceGroupById(int id)
    {
        var group = await unit.Repository<DeviceGroup>().GetByIdAsync(id);

        if(group == null) return NotFound();

        return group;
    }




    [HttpPost]
    public async Task<ActionResult<DeviceGroup>> CreateDeviceGroup(DeviceGroup group)
    {
        unit.Repository<DeviceGroup>().Add(group);

        if(await unit.Complete())
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

        unit.Repository<DeviceGroup>().Update(group);

        if(await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem updating the group");

    }



    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteGroup(int id)
    {
        var group = await unit.Repository<DeviceGroup>().GetByIdAsync(id);

        if(group == null) return NotFound();

        unit.Repository<DeviceGroup>().Remove(group);

         if(await unit.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting the group");
    }
    private bool GroupExists(int id)
    {
        return unit.Repository<DeviceGroup>().Exists(id);
    }

    
}