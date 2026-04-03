using DeviceManager.Core.Exceptions;
using DeviceManager.Core.DTOs;
using DeviceManager.Core.Interfaces;
using DeviceManager.Core.Models;

namespace DeviceManager.Infrastructure.Services;

/// <summary>
/// Business logic layer for device operations.
/// Handles DTO-to-model mapping, duplicate validation, and user lookups for assigned devices.
/// </summary>
public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IUserRepository _userRepository;

    public DeviceService(IDeviceRepository deviceRepository, IUserRepository userRepository)
    {
        _deviceRepository = deviceRepository;
        _userRepository = userRepository;
    }

    public async Task<List<DeviceDto>> GetAllDevicesAsync()
    {
        var devices = await _deviceRepository.GetAllAsync();
        var deviceDtos = new List<DeviceDto>();

        foreach (var device in devices)
        {
            var dto = MapToDto(device);

            // Resolve assigned user name if a user is assigned
            if (!string.IsNullOrEmpty(device.AssignedUserId))
            {
                var user = await _userRepository.GetByIdAsync(device.AssignedUserId);
                dto.AssignedUserName = user?.Name;
            }

            deviceDtos.Add(dto);
        }

        return deviceDtos;
    }

    public async Task<DeviceDto?> GetDeviceByIdAsync(string id)
    {
        var device = await _deviceRepository.GetByIdAsync(id);
        if (device == null)
            throw new NotFoundException("Device", id);

        var dto = MapToDto(device);

        if (!string.IsNullOrEmpty(device.AssignedUserId))
        {
            var user = await _userRepository.GetByIdAsync(device.AssignedUserId);
            dto.AssignedUserName = user?.Name;
        }

        return dto;
    }

    public async Task<DeviceDto> CreateDeviceAsync(CreateDeviceDto createDto)
    {
        // Check for duplicate device (same name + manufacturer)
        var existing = await _deviceRepository.GetByNameAndManufacturerAsync(
            createDto.Name, createDto.Manufacturer);

        if (existing != null)
        {
            throw new ConflictException(
                $"A device with name '{createDto.Name}' from '{createDto.Manufacturer}' already exists.");
        }

        var device = new Device
        {
            Name = createDto.Name,
            Manufacturer = createDto.Manufacturer,
            Type = createDto.Type,
            Os = createDto.Os,
            OsVersion = createDto.OsVersion,
            Processor = createDto.Processor,
            RamAmount = createDto.RamAmount,
            Location = createDto.Location,
            Description = createDto.Description
        };

        var created = await _deviceRepository.CreateAsync(device);
        return MapToDto(created);
    }

    public async Task<bool> UpdateDeviceAsync(string id, UpdateDeviceDto updateDto)
    {
        var existing = await _deviceRepository.GetByIdAsync(id);
        if (existing == null)
            throw new NotFoundException("Device", id);

        // Check for duplicates only if name or manufacturer changed
        if (!string.Equals(existing.Name, updateDto.Name, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(existing.Manufacturer, updateDto.Manufacturer, StringComparison.OrdinalIgnoreCase))
        {
            var duplicate = await _deviceRepository.GetByNameAndManufacturerAsync(
                updateDto.Name, updateDto.Manufacturer);

            if (duplicate != null && duplicate.Id != id)
            {
                throw new ConflictException(
                    $"A device with name '{updateDto.Name}' from '{updateDto.Manufacturer}' already exists.");
            }
        }

        existing.Name = updateDto.Name;
        existing.Manufacturer = updateDto.Manufacturer;
        existing.Type = updateDto.Type;
        existing.Os = updateDto.Os;
        existing.OsVersion = updateDto.OsVersion;
        existing.Processor = updateDto.Processor;
        existing.RamAmount = updateDto.RamAmount;
        existing.Location = updateDto.Location;
        existing.Description = updateDto.Description;

        return await _deviceRepository.UpdateAsync(id, existing);
    }

    public async Task<bool> DeleteDeviceAsync(string id)
    {
        var deleted = await _deviceRepository.DeleteAsync(id);
        if (!deleted)
            throw new NotFoundException("Device", id);
            
        return true;
    }

    /// <summary>
    /// Maps a Device domain model to a DeviceDto.
    /// </summary>
    private static DeviceDto MapToDto(Device device)
    {
        return new DeviceDto
        {
            Id = device.Id,
            Name = device.Name,
            Manufacturer = device.Manufacturer,
            Type = device.Type,
            Os = device.Os,
            OsVersion = device.OsVersion,
            Processor = device.Processor,
            RamAmount = device.RamAmount,
            Description = device.Description,
            Location = device.Location,
            AssignedUserId = device.AssignedUserId,
            CreatedAt = device.CreatedAt,
            UpdatedAt = device.UpdatedAt
        };
    }
}
