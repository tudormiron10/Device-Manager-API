using DeviceManager.Core.Exceptions;
using DeviceManager.Core.DTOs;
using DeviceManager.Core.Interfaces;
using DeviceManager.Core.Models;
using DeviceManager.Infrastructure.Helpers;

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
            if (!string.IsNullOrEmpty(device.AssignedUserId))
            {
                var user = await _userRepository.GetByIdAsync(device.AssignedUserId);
                dto.AssignedUserName = user?.Name;
                if (user != null) dto.Location = user.Location;
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
            if (user != null) dto.Location = user.Location;
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
        var device = await _deviceRepository.GetByIdAsync(id);
        if (device == null)
            throw new NotFoundException("Device", id);

        // Security check: cannot delete a device that is currently assigned
        if (!string.IsNullOrEmpty(device.AssignedUserId))
        {
            throw new ConflictException("Cannot delete an asset that is currently assigned to a user. Please unassign it first in the logistics section.");
        }

        var deleted = await _deviceRepository.DeleteAsync(id);
        if (!deleted)
            throw new NotFoundException("Device", id);
            
        return true;
    }

    public async Task<DeviceDto> AssignDeviceAsync(string deviceId, string userId)
    {
        var device = await _deviceRepository.GetByIdAsync(deviceId);
        if (device == null)
            throw new NotFoundException("Device", deviceId);

        // Device already assigned to someone else
        if (!string.IsNullOrEmpty(device.AssignedUserId) && device.AssignedUserId != userId)
            throw new ConflictException("This device is already assigned to another user.");

        var updated = await _deviceRepository.AssignDeviceAsync(deviceId, userId)
            ?? throw new NotFoundException("Device", deviceId);

        var dto = MapToDto(updated);
        var user = await _userRepository.GetByIdAsync(userId);
        dto.AssignedUserName = user?.Name;
        if (user != null) dto.Location = user.Location;
        return dto;
    }

    public async Task<DeviceDto> UnassignDeviceAsync(string deviceId, string userId)
    {
        var device = await _deviceRepository.GetByIdAsync(deviceId);
        if (device == null)
            throw new NotFoundException("Device", deviceId);

        // Only the user who assigned it can unassign it
        if (device.AssignedUserId != userId)
            throw new ForbiddenException("You can only unassign a device that is assigned to you.");

        var updated = await _deviceRepository.UnassignDeviceAsync(deviceId)
            ?? throw new NotFoundException("Device", deviceId);

        return MapToDto(updated);
    }

    public async Task<List<DeviceDto>> SearchDevicesAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return await GetAllDevicesAsync();

        var devices = await _deviceRepository.GetAllAsync();
        
        // Normalize and tokenize (keep significant tokens only)
        var tokens = query.ToLowerInvariant()
            .Split(new[] { ' ', ',', '.', '-', '_', '/' }, StringSplitOptions.RemoveEmptyEntries)
            .Where(t => t.Length >= 2)
            .ToList();

        if (tokens.Count == 0) return new List<DeviceDto>();

        var scoredDevices = new List<(Device Device, int Score)>();

        foreach (var device in devices)
        {
            int score = 0;
            foreach (var token in tokens)
            {
                // Each field has its own significance weight
                score += CalculateFieldScore(device.Name, token, 20);
                score += CalculateFieldScore(device.Manufacturer, token, 15);
                score += CalculateFieldScore(device.Processor, token, 10);
                score += CalculateFieldScore(device.RamAmount, token, 5);
            }

            // Quality threshold: Similarity-based search naturally excludes "noise" (like "Ana", "are", "mere")
            // as they won't match any hardware terms with a similarity > 0.3
            if (score >= 10)
            {
                scoredDevices.Add((device, score));
            }
        }

        var results = scoredDevices
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.Device.Name)
            .Select(x => x.Device)
            .ToList();

        // Enriched Mapping
        var deviceDtos = new List<DeviceDto>();
        foreach (var device in results)
        {
            var dto = MapToDto(device);
            if (!string.IsNullOrEmpty(device.AssignedUserId))
            {
                var user = await _userRepository.GetByIdAsync(device.AssignedUserId);
                dto.AssignedUserName = user?.Name;
                if (user != null) dto.Location = user.Location;
            }
            deviceDtos.Add(dto);
        }

        return deviceDtos;
    }

    /// <summary>
    /// Calculates a weighted score for a field based on match quality.
    /// Uses Levenshtein similarity to handle typos and noise.
    /// </summary>
    private static int CalculateFieldScore(string fieldValue, string token, int weight)
    {
        if (string.IsNullOrWhiteSpace(fieldValue) || string.IsNullOrWhiteSpace(token)) return 0;

        // Split field into words (e.g. "Galaxy Tab S10 Ultra" -> ["Galaxy", "Tab", "S10", "Ultra"])
        var words = fieldValue.Split(new[] { ' ', ',', '.', '-', '_', '/' }, StringSplitOptions.RemoveEmptyEntries);
        int maxWordScore = 0;

        foreach (var word in words)
        {
            int currentScore = 0;

            // 1. Exact Match (Highest priority)
            if (string.Equals(word, token, StringComparison.OrdinalIgnoreCase))
            {
                currentScore = weight * 5;
            }
            else
            {
                // 2. Fuzzy Similarity (Handles typos like "Samsug")
                double similarity = StringSimilarityHelper.GetSimilarity(word, token);
                if (similarity >= 0.8)
                {
                    currentScore = (int)(weight * 3 * similarity);
                }
                // 3. Partial Containment (Fallback for longer tokens)
                else if (token.Length >= 3 && word.Contains(token, StringComparison.OrdinalIgnoreCase))
                {
                    currentScore = weight;
                }
            }

            if (currentScore > maxWordScore) maxWordScore = currentScore;
        }

        return maxWordScore;
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
