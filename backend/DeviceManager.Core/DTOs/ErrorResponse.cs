namespace DeviceManager.Core.DTOs;

/// <summary>
/// Standardized error response returned by the API for all error cases.
/// Ensures Angular frontend always receives a consistent JSON shape.
/// </summary>
public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = null!;
    public string? Details { get; set; }
}
