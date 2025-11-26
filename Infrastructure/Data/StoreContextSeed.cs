
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.Brands.Any())
            {
                var brandsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/brands.json");

                var brands = JsonSerializer.Deserialize<List<Brand>>(brandsData);
           
                if(brands == null) return;

                context.Brands.AddRange(brands);

                await context.SaveChangesAsync();
           
            }
            
            if (!context.Groups.Any())
            {
                var groupsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/devicegroups.json");
                var groups = JsonSerializer.Deserialize<List<DeviceGroup>>(groupsData);
                if (groups != null)
                    context.Groups.AddRange(groups);

                await context.SaveChangesAsync();
            }

            if (!context.Devices.Any())
            {
                var devicesData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/devices.json");
                var devices = JsonSerializer.Deserialize<List<Device>>(devicesData);
                if (devices != null)
                    context.Devices.AddRange(devices);

                await context.SaveChangesAsync();
            }


        }
    }
}