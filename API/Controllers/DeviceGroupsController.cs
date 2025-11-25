
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeviceGroupsController : ControllerBase
{
    private readonly StoreContext context;

    public DeviceGroupsController(StoreContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeviceGroup>>> GetDeviceGroup()
    {
        return await context.Groups.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DeviceGroup>> GetDeviceGroupById(int id)
    {
        var group = await context.Groups.FindAsync(id);

        if(group == null) return NotFound();

        return group;
    }

    [HttpPost]
    public async Task<ActionResult<DeviceGroup>> CreateDeviceGroup(DeviceGroup group)
    {
        context.Groups.Add(group);

        await context.SaveChangesAsync();

        return group;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateDeviceGroup(int id,DeviceGroup group)
    {
        if(group.Id != id || !GroupExists(id))
            return BadRequest("Cannot update group");


        context.Entry(group).State = EntityState.Modified;

        await context.SaveChangesAsync();

        return NoContent();
    }


    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteGroup(int id)
    {
        var group = await context.Groups.FindAsync(id);

        if(group == null) return NotFound();

        context.Groups.Remove(group);

        await context.SaveChangesAsync();

        return NoContent();
    }
    private bool GroupExists(int id)
    {
        return context.Groups.Any(x => x.Id == id);
    }

    
}