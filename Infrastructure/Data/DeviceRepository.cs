
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

        public async Task<IReadOnlyList<Brand>> GetBrandsAsync()
        {
            return await context.Brands
            .OrderBy(x => x.BrandName)
            .ToListAsync();

        }

        public async Task<IReadOnlyList<DeviceGroup>> GetGroupsAsync()
        {
            return await context.Groups
            .OrderBy(x => x.GroupName)
            .ToListAsync();
        }

        public async Task<Device?> GetDeviceByIdAsync(int id)
        {
               return await context.Devices
                .Include(d => d.Brand)
                .Include(d => d.DeviceGroup)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IReadOnlyList<Device>> GetDevicesAsync(string? brand, string? deviceGroup
        , string? sort)
        {
            var query = context.Devices
                .Include(d => d.Brand)
                .Include(d => d.DeviceGroup)
                .AsQueryable();

            if (!string.IsNullOrEmpty(brand))
                query = query.Where(d => d.Brand!.BrandName == brand);

            if (!string.IsNullOrEmpty(deviceGroup))
                query = query.Where(d => d.DeviceGroup!.GroupName == deviceGroup);

            query = sort switch
            {
                "priceAsc" => query.OrderBy(x => x.Price),
                "priceDesc" => query.OrderByDescending(x => x.Price),
                _ => query.OrderBy(x => x.Id)
            };
            

            return await query.ToListAsync();
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