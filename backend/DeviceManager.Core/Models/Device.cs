using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeviceManager.Core.Models;

public class Device
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("name")]
    public string Name { get; set; } = null!;

    [BsonElement("manufacturer")]
    public string Manufacturer { get; set; } = null!;

    [BsonElement("type")]
    public string Type { get; set; } = null!;

    [BsonElement("os")]
    public string Os { get; set; } = null!;

    [BsonElement("osVersion")]
    public string OsVersion { get; set; } = null!;

    [BsonElement("processor")]
    public string Processor { get; set; } = null!;

    [BsonElement("ramAmount")]
    public string RamAmount { get; set; } = null!;

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("location")]
    public string? Location { get; set; }

    [BsonElement("assignedUserId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? AssignedUserId { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
