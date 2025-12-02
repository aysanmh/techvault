
using Core.Entities;

namespace Core.Interfaces
{
    public interface IDeviceRepository
    {
        Task<IReadOnlyList<Device>> GetDevicesAsync(string? brand, string? deviceGroup,string? sort);

        Task<Device?> GetDeviceByIdAsync(int id);

        Task<IReadOnlyList<Brand>> GetBrandsAsync();

        Task<IReadOnlyList<DeviceGroup>> GetGroupsAsync();
        void AddDevice(Device device);

        void UpdateDevice(Device device);

        void DeleteDevice(Device device);

        Task<bool> SaveChangesAsync();
    }
}