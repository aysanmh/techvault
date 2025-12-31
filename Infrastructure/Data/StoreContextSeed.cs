
using System.Text.Json;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, UserManager<AppUser> userManager)
        {


            
        if (!userManager.Users.Any(x => x.UserName == "admin@test.com"))
            {
                var user = new AppUser
                {
                    UserName = "admin@test.com",
                    Email = "admin@test.com"
                };

                var result = await userManager.CreateAsync(user, "Pa$$w0rd");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    throw new Exception("Error creating admin user.");
                }
            }
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

            if (!context.DeliveryMethods.Any())
            {
                var deliveryMethodData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodData);
                if (deliveryMethods != null)
                    context.DeliveryMethods.AddRange(deliveryMethods);

                await context.SaveChangesAsync();
            }
       


        }
    }
}