using DeviceManager.Core.Models;

namespace DeviceManager.Core.Interfaces;

/// <summary>
/// Repository contract for User CRUD operations against the database.
/// </summary>
public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
    Task<bool> UpdateAsync(string id, User user);
    Task<bool> DeleteAsync(string id);
}
