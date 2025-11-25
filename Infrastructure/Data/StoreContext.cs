
using Core.Entities;
using Infrastructure.config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

    public class StoreContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Device> Devices { get; set; }

    public DbSet<DeviceGroup> Groups { get; set; }

    public DbSet<Brand> Brands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeviceConfiguration).Assembly);
    }
}
