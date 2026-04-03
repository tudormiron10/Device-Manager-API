using DeviceManager.Core.DTOs;
using DeviceManager.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    /// <summary>
    /// GET /api/devices - Returns all devices with assigned user info.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<DeviceDto>>> GetAll()
    {
        var devices = await _deviceService.GetAllDevicesAsync();
        return Ok(devices);
    }

    /// <summary>
    /// GET /api/devices/{id} - Returns a single device by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<DeviceDto>> GetById(string id)
    {
        var device = await _deviceService.GetDeviceByIdAsync(id);
        return Ok(device);
    }

    /// <summary>
    /// POST /api/devices - Creates a new device.
    /// Validates that the device does not already exist (by name + manufacturer).
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<DeviceDto>> Create([FromBody] CreateDeviceDto dto)
    {
        var created = await _deviceService.CreateDeviceAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// PUT /api/devices/{id} - Updates an existing device.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, [FromBody] UpdateDeviceDto dto)
    {
        await _deviceService.UpdateDeviceAsync(id, dto);
        return NoContent();
    }

    /// <summary>
    /// DELETE /api/devices/{id} - Deletes a device.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        await _deviceService.DeleteDeviceAsync(id);
        return NoContent();
    }
}
