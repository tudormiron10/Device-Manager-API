namespace DeviceManager.Core.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Role { get; set; } = null!;
}
