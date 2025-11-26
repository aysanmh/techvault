
using Core.Entities;

namespace Core.Interfaces
{
    public interface IDeviceGroupRepository
    {

        Task<IReadOnlyList<DeviceGroup>> GetDeviceGroupsAsync();

        Task<DeviceGroup?> GetDeviceGroupByIdAsync(int id);

        Task<IReadOnlyList<Device>> GetDevicesAsync(int groupId);
        void AddDeviceGroup(DeviceGroup deviceGroup);

        void UpdateDeviceGroup(DeviceGroup deviceGroup);

        void DeleteDeviceGroup(DeviceGroup deviceGroup);

        bool DeviceGroupExists(int id);

        Task<bool> SaveChangesAsync();
        
    }
}