using DeviceManager.Core.Interfaces;
using DeviceManager.Core.Models;
using DeviceManager.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DeviceManager.Infrastructure.Repositories;

/// <summary>
/// MongoDB implementation of the device repository.
/// All operations use async MongoDB driver methods.
/// </summary>
public class DeviceRepository : IDeviceRepository
{
    private readonly IMongoCollection<Device> _devices;

    public DeviceRepository(MongoDbContext context)
    {
        _devices = context.Devices;
    }

    public async Task<List<Device>> GetAllAsync()
    {
        return await _devices.Find(_ => true).ToListAsync();
    }

    public async Task<Device?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return null;

        return await _devices.Find(d => d.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Device?> GetByNameAndManufacturerAsync(string name, string manufacturer)
    {
        var filter = Builders<Device>.Filter.And(
            Builders<Device>.Filter.Regex(d => d.Name, new BsonRegularExpression($"^{name}$", "i")),
            Builders<Device>.Filter.Regex(d => d.Manufacturer, new BsonRegularExpression($"^{manufacturer}$", "i"))
        );
        return await _devices.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<Device> CreateAsync(Device device)
    {
        device.CreatedAt = DateTime.UtcNow;
        device.UpdatedAt = DateTime.UtcNow;
        await _devices.InsertOneAsync(device);
        return device;
    }

    public async Task<bool> UpdateAsync(string id, Device device)
    {
        if (!ObjectId.TryParse(id, out _))
            return false;

        device.UpdatedAt = DateTime.UtcNow;

        var update = Builders<Device>.Update
            .Set(d => d.Name, device.Name)
            .Set(d => d.Manufacturer, device.Manufacturer)
            .Set(d => d.Type, device.Type)
            .Set(d => d.Os, device.Os)
            .Set(d => d.OsVersion, device.OsVersion)
            .Set(d => d.Processor, device.Processor)
            .Set(d => d.RamAmount, device.RamAmount)
            .Set(d => d.Description, device.Description)
            .Set(d => d.Location, device.Location)
            .Set(d => d.UpdatedAt, device.UpdatedAt);

        var result = await _devices.UpdateOneAsync(d => d.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            return false;

        var result = await _devices.DeleteOneAsync(d => d.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<Device?> AssignDeviceAsync(string deviceId, string userId)
    {
        if (!ObjectId.TryParse(deviceId, out _))
            return null;

        var update = Builders<Device>.Update
            .Set(d => d.AssignedUserId, userId)
            .Set(d => d.UpdatedAt, DateTime.UtcNow);

        var result = await _devices.FindOneAndUpdateAsync(
            d => d.Id == deviceId,
            update,
            new FindOneAndUpdateOptions<Device> { ReturnDocument = ReturnDocument.After });

        return result;
    }

    public async Task<Device?> UnassignDeviceAsync(string deviceId)
    {
        if (!ObjectId.TryParse(deviceId, out _))
            return null;

        var update = Builders<Device>.Update
            .Set(d => d.AssignedUserId, (string?)null)
            .Set(d => d.UpdatedAt, DateTime.UtcNow);

        var result = await _devices.FindOneAndUpdateAsync(
            d => d.Id == deviceId,
            update,
            new FindOneAndUpdateOptions<Device> { ReturnDocument = ReturnDocument.After });

        return result;
    }
}
