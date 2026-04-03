namespace DeviceManager.Core.DTOs;

/// <summary>
/// Response DTO for device data. Includes flattened assigned user info.
/// </summary>
public class DeviceDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Manufacturer { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Os { get; set; } = null!;
    public string OsVersion { get; set; } = null!;
    public string Processor { get; set; } = null!;
    public string RamAmount { get; set; } = null!;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? AssignedUserId { get; set; }
    public string? AssignedUserName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
