using System.ComponentModel.DataAnnotations;

namespace DeviceManager.Core.DTOs;

/// <summary>
/// Request DTO for updating an existing device. All fields are required.
/// </summary>
public class UpdateDeviceDto
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Manufacturer is required")]
    public string Manufacturer { get; set; } = null!;

    [Required(ErrorMessage = "Type is required")]
    [RegularExpression("^(phone|tablet)$", ErrorMessage = "Type must be 'phone' or 'tablet'")]
    public string Type { get; set; } = null!;

    [Required(ErrorMessage = "OS is required")]
    public string Os { get; set; } = null!;

    [Required(ErrorMessage = "OS Version is required")]
    public string OsVersion { get; set; } = null!;

    [Required(ErrorMessage = "Processor is required")]
    public string Processor { get; set; } = null!;

    [Required(ErrorMessage = "RAM Amount is required")]
    public string RamAmount { get; set; } = null!;

    public string? Location { get; set; }

    public string? Description { get; set; }
}
