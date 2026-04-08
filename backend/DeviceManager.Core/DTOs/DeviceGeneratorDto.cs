namespace DeviceManager.Core.DTOs;

/// <summary>
/// DTO sent to the AI service to generate a device description.
/// Contains minimal technical specifications.
/// </summary>
public class DeviceGeneratorDto
{
    public string Name { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Os { get; set; } = string.Empty;
    public string OsVersion { get; set; } = string.Empty;
    public string Processor { get; set; } = string.Empty;
    public string RamAmount { get; set; } = string.Empty;
}
