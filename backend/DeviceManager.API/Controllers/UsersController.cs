using DeviceManager.Core.DTOs;
using DeviceManager.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// GET /api/users - Returns all users.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAll()
    {
        var users = await _userRepository.GetAllAsync();
        var userDtos = users.Select(u => MapToDto(u)).ToList();
        return Ok(userDtos);
    }

    /// <summary>
    /// GET /api/users/{id} - Returns a single user by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(string id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new DeviceManager.Core.Exceptions.NotFoundException("User", id);

        return Ok(MapToDto(user));
    }

    /// <summary>
    /// Maps a User domain model to a UserDto, excluding sensitive fields.
    /// </summary>
    private static UserDto MapToDto(Core.Models.User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            Location = user.Location,
            CreatedAt = user.CreatedAt
        };
    }
}
