namespace DeviceManager.Core.DTOs;

/// <summary>
/// Response DTO for user data. Excludes sensitive fields like passwordHash.
/// </summary>
public class UserDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string Location { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
