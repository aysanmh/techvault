
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DeviceGroupRepository(StoreContext context) : IDeviceGroupRepository
    {

        public void AddDeviceGroup(DeviceGroup deviceGroup)
        {
             context.Groups.Add(deviceGroup);
        }

        public void DeleteDeviceGroup(DeviceGroup deviceGroup)
        {
            context.Groups.Remove(deviceGroup);
        }

        public bool DeviceGroupExists(int id)
        {
            return context.Groups.Any(x => x.Id == id);
        }

        public async  Task<DeviceGroup?> GetDeviceGroupByIdAsync(int id)
        {
            return await context.Groups.FindAsync(id);
        }

        public async Task<IReadOnlyList<DeviceGroup>> GetDeviceGroupsAsync()
        {
            return await context.Groups.ToListAsync();
        }

       

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void UpdateDeviceGroup(DeviceGroup deviceGroup)
        {
            context.Entry(deviceGroup).State = EntityState.Modified;
        }
    }
}