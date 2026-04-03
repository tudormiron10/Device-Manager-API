using DeviceManager.Core.DTOs;

namespace DeviceManager.Core.Interfaces;

/// <summary>
/// Service contract for device business logic.
/// Handles DTO mapping, validation, and orchestration between repositories.
/// </summary>
public interface IDeviceService
{
    Task<List<DeviceDto>> GetAllDevicesAsync();
    Task<DeviceDto?> GetDeviceByIdAsync(string id);
    Task<DeviceDto> CreateDeviceAsync(CreateDeviceDto dto);
    Task<bool> UpdateDeviceAsync(string id, UpdateDeviceDto dto);
    Task<bool> DeleteDeviceAsync(string id);
}
