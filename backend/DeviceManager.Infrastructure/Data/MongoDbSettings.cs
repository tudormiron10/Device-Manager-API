namespace DeviceManager.Infrastructure.Data;

/// <summary>
/// Configuration model for MongoDB connection.
/// Maps to the "MongoDbSettings" section in appsettings.json.
/// </summary>
public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}
