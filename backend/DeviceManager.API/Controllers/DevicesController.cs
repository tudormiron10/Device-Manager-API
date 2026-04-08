using DeviceManager.Core.DTOs;
using DeviceManager.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeviceManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;
    private readonly IGeneratorService _generatorService;

    public DevicesController(IDeviceService deviceService, IGeneratorService generatorService)
    {
        _deviceService = deviceService;
        _generatorService = generatorService;
    }

    /// <summary>
    /// GET /api/devices - Returns all devices with assigned user info. Public endpoint.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<DeviceDto>>> GetAll()
    {
        var devices = await _deviceService.GetAllDevicesAsync();
        return Ok(devices);
    }

    /// <summary>
    /// GET /api/devices/{id} - Returns a single device by ID. Public endpoint.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<DeviceDto>> GetById(string id)
    {
        var device = await _deviceService.GetDeviceByIdAsync(id);
        return Ok(device);
    }

    /// <summary>
    /// POST /api/devices - Creates a new device. Requires elevated permissions.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Hardware Specialist,Project Manager")]
    public async Task<ActionResult<DeviceDto>> Create([FromBody] CreateDeviceDto dto)
    {
        var created = await _deviceService.CreateDeviceAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// PUT /api/devices/{id} - Updates an existing device. Requires elevated permissions.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Hardware Specialist,Project Manager")]
    public async Task<ActionResult> Update(string id, [FromBody] UpdateDeviceDto dto)
    {
        await _deviceService.UpdateDeviceAsync(id, dto);
        return NoContent();
    }

    /// <summary>
    /// DELETE /api/devices/{id} - Deletes a device. Requires elevated permissions.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Hardware Specialist,Project Manager")]
    public async Task<ActionResult> Delete(string id)
    {
        await _deviceService.DeleteDeviceAsync(id);
        return NoContent();
    }

    /// <summary>
    /// POST /api/devices/{id}/assign - Assigns the device to the authenticated user.
    /// Fails if the device is already assigned to a different user.
    /// </summary>
    [HttpPost("{id}/assign")]
    [Authorize]
    public async Task<ActionResult<DeviceDto>> Assign(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)!;

        var device = await _deviceService.AssignDeviceAsync(id, userId);
        return Ok(device);
    }

    /// <summary>
    /// POST /api/devices/{id}/unassign - Unassigns the device from the authenticated user.
    /// Fails if the device is assigned to a different user.
    /// </summary>
    [HttpPost("{id}/unassign")]
    [Authorize]
    public async Task<ActionResult<DeviceDto>> Unassign(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)!;

        var device = await _deviceService.UnassignDeviceAsync(id, userId);
        return Ok(device);
    }

    /// <summary>
    /// POST /api/devices/generate-description - Generates an AI description for a device.
    /// </summary>
    [HttpPost("generate-description")]
    [Authorize]
    public async Task<ActionResult<string>> GenerateDescription([FromBody] DeviceGeneratorDto dto)
    {
        var description = await _generatorService.GenerateDeviceDescriptionAsync(dto);
        return Ok(new { description });
    }
}
