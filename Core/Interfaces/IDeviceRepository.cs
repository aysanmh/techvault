
using Core.Entities;

namespace Core.Interfaces
{
    public interface IDeviceRepository
    {
        Task<IReadOnlyList<Device>> GetDevicesAsync();

        Task<Device?> GetDeviceByIdAsync(int id);

        void AddDevice(Device device);

        void UpdateDevice(Device device);

        void DeleteDevice(Device device);

        Task<bool> SaveChangesAsync();
    }
}