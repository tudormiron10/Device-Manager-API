using DeviceManager.Core.Models;

namespace DeviceManager.Core.Interfaces;

/// <summary>
/// Repository contract for Device CRUD operations against the database.
/// </summary>
public interface IDeviceRepository
{
    Task<List<Device>> GetAllAsync();
    Task<Device?> GetByIdAsync(string id);
    Task<Device?> GetByNameAndManufacturerAsync(string name, string manufacturer);
    Task<Device> CreateAsync(Device device);
    Task<bool> UpdateAsync(string id, Device device);
    Task<bool> DeleteAsync(string id);
}
