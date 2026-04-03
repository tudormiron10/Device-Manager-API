using DeviceManager.Core.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DeviceManager.Infrastructure.Data;

/// <summary>
/// Provides typed access to MongoDB collections.
/// Registered as a singleton to reuse the underlying MongoDB connection pool.
/// </summary>
public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<Device> Devices => _database.GetCollection<Device>("devices");
    public IMongoCollection<User> Users => _database.GetCollection<User>("users");
}
