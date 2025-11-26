
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DeviceRepository(StoreContext context) : IDeviceRepository
    {

        public void AddDevice(Device device)
        {
            context.Devices.Add(device);
        }

        public void DeleteDevice(Device device)
        {
            context.Devices.Remove(device);
        }


        public async Task<Device?> GetDeviceByIdAsync(int id)
        {
               return await context.Devices
                .Include(d => d.Brand)
                .Include(d => d.Group)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IReadOnlyList<Device>> GetDevicesAsync()
        {
             return await context.Devices
                .Include(d => d.Brand)
                .Include(d => d.Group)
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void UpdateDevice(Device device)
        {
            context.Entry(device).State = EntityState.Modified;
        }
    }
}