using DeviceManager.Core.DTOs;

namespace DeviceManager.Core.Interfaces;

/// <summary>
/// Service for AI-powered content generation.
/// Used to create human-readable device descriptions from technical specifications.
/// </summary>
public interface IGeneratorService
{
    /// <summary>
    /// Generates a concise, professional description for a device based on its hardware specs.
    /// </summary>
    /// <param name="specs">The technical specifications of the device.</param>
    /// <returns>A string containing the generated description.</returns>
    Task<string> GenerateDeviceDescriptionAsync(DeviceGeneratorDto specs);
}
