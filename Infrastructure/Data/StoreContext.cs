
using Core.Entities;
using Infrastructure.config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

    public class StoreContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Device> Devices { get; set; }

    public DbSet<DeviceGroup> Groups { get; set; }

    public DbSet<Brand> Brands { get; set; }

    public DbSet<Address> Addresses { get; set; }

    public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeviceConfiguration).Assembly);
    }
}
