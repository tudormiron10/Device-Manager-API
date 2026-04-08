using DeviceManager.Core.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;
using BC = BCrypt.Net.BCrypt;

namespace DeviceManager.Infrastructure.Data;

/// <summary>
/// Seed the database with initial data.
/// </summary>
public class MongoDbSeeder
{
    private readonly MongoDbContext _context;

    public MongoDbSeeder(MongoDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        Console.WriteLine("[SEEDER] Starting seeding process...");
        await SeedUsersAsync();
        await SeedDevicesAsync();
        Console.WriteLine("[SEEDER] Seeding process completed.");
    }

    private async Task SeedUsersAsync()
    {
        var count = await _context.Users.CountDocumentsAsync(new BsonDocument());
        if (count > 0)
        {
            Console.WriteLine($"[SEEDER] Users already exist ({count}). Skipping user seeding.");
            return;
        }

        Console.WriteLine("[SEEDER] Seeding users...");
        var users = new List<User>
        {
            new User { Name = "John Smith", Email = "hardware@test.com", PasswordHash = BC.HashPassword("test123!"), Role = "Hardware Specialist", Location = "Central Warehouse A" },
            new User { Name = "Ana Popescu", Email = "manager@test.com", PasswordHash = BC.HashPassword("test123!"), Role = "Project Manager", Location = "HQ - Logistics Office" },
            new User { Name = "Tudor Miron", Email = "dev@test.com", PasswordHash = BC.HashPassword("test123!"), Role = "Developer", Location = "HQ - Dev Dept" },
            new User { Name = "Elena Ionescu", Email = "qa@test.com", PasswordHash = BC.HashPassword("test123!"), Role = "QA Engineer", Location = "Tech Lab" },
            new User { Name = "Mihai Stoica", Email = "designer@test.com", PasswordHash = BC.HashPassword("test123!"), Role = "Designer", Location = "HQ - Creative Dept" },
            new User { Name = "Alex Dumitru", Email = "devops@test.com", PasswordHash = BC.HashPassword("test123!"), Role = "DevOps Engineer", Location = "Server Room 1" },
            new User { Name = "Sarah Connor", Email = "qa2@test.com", PasswordHash = BC.HashPassword("test123!"), Role = "QA Engineer", Location = "Tech Lab" },
            new User { Name = "James Bond", Email = "dev2@test.com", PasswordHash = BC.HashPassword("test123!"), Role = "Developer", Location = "HQ - Dev Dept" }
        };

        await _context.Users.InsertManyAsync(users);
        Console.WriteLine($"[SEEDER] Inserted {users.Count} users.");
    }

    private async Task SeedDevicesAsync()
    {
        var count = await _context.Devices.CountDocumentsAsync(new BsonDocument());
        if (count > 0) return;

        var users = await _context.Users.Find(new BsonDocument()).ToListAsync();

        var devices = new List<Device>
        {
            // Phones
            new Device { Name = "iPhone 16 Pro", Manufacturer = "APPLE", Type = "Phone", Os = "iOS", OsVersion = "18", Processor = "A18 Pro", RamAmount = "8GB", Location = "Shelf A1" },
            new Device { Name = "iPhone 15", Manufacturer = "APPLE", Type = "Phone", Os = "iOS", OsVersion = "17", Processor = "A16 Bionic", RamAmount = "6GB", Location = "Shelf A1" },
            new Device { Name = "Galaxy S25 Ultra", Manufacturer = "SAMSUNG", Type = "Phone", Os = "Android", OsVersion = "15", Processor = "Snapdragon 8 Gen 4", RamAmount = "12GB", Location = "Shelf A2" },
            new Device { Name = "Galaxy S24", Manufacturer = "SAMSUNG", Type = "Phone", Os = "Android", OsVersion = "14", Processor = "Exynos 2400", RamAmount = "8GB", Location = "Shelf A2" },
            new Device { Name = "Xiaomi 15 Pro", Manufacturer = "XIAOMI", Type = "Phone", Os = "Android", OsVersion = "15", Processor = "Snapdragon 8 Gen 4", RamAmount = "16GB", Location = "Shelf A3" },
            new Device { Name = "Pixel 8 Pro", Manufacturer = "GOOGLE", Type = "Phone", Os = "Android", OsVersion = "14", Processor = "Tensor G3", RamAmount = "12GB", Location = "Shelf A4" },
            new Device { Name = "OnePlus 12", Manufacturer = "ONEPLUS", Type = "Phone", Os = "Android", OsVersion = "14", Processor = "Snapdragon 8 Gen 3", RamAmount = "16GB", Location = "Shelf A3" },
            new Device { Name = "Nothing Phone 2", Manufacturer = "NOTHING", Type = "Phone", Os = "Android", OsVersion = "13", Processor = "Snapdragon 8+ Gen 1", RamAmount = "12GB", Location = "Shelf A4" },
            
            // Tablets
            new Device { Name = "Galaxy Tab S10 Ultra", Manufacturer = "SAMSUNG", Type = "Tablet", Os = "Android", OsVersion = "14", Processor = "Dimensity 9300+", RamAmount = "12GB", Location = "Shelf B1" },
            new Device { Name = "iPad Pro M4", Manufacturer = "APPLE", Type = "Tablet", Os = "iPadOS", OsVersion = "18", Processor = "M4", RamAmount = "8GB", Location = "Shelf B1" },
            new Device { Name = "iPad Air", Manufacturer = "APPLE", Type = "Tablet", Os = "iPadOS", OsVersion = "17", Processor = "M2", RamAmount = "8GB", Location = "Shelf B1" },
            new Device { Name = "Surface Pro 11", Manufacturer = "MICROSOFT", Type = "Tablet", Os = "Windows", OsVersion = "11", Processor = "Snapdragon X Elite", RamAmount = "16GB", Location = "Tech Lab" },
            new Device { Name = "Pixel Tablet 2", Manufacturer = "GOOGLE", Type = "Tablet", Os = "Android", OsVersion = "15", Processor = "Tensor G4", RamAmount = "8GB", Location = "Shelf B2" },
            new Device { Name = "Lenovo Tab P12", Manufacturer = "LENOVO", Type = "Tablet", Os = "Android", OsVersion = "13", Processor = "MediaTek Dimensity 7050", RamAmount = "8GB", Location = "Shelf B2" },

            // Laptops
            new Device { Name = "Dell XPS 15", Manufacturer = "DELL", Type = "Laptop", Os = "Windows", OsVersion = "11", Processor = "Intel Core Ultra 9", RamAmount = "32GB", Location = "HQ - Dev Dept" },
            new Device { Name = "MacBook Pro M4", Manufacturer = "APPLE", Type = "Laptop", Os = "macOS", OsVersion = "Sequoia", Processor = "M4 Max", RamAmount = "64GB", Location = "HQ - Creative Dept" },
            new Device { Name = "MacBook Air M3", Manufacturer = "APPLE", Type = "Laptop", Os = "macOS", OsVersion = "Sonoma", Processor = "M3", RamAmount = "16GB", Location = "HQ - Logistics Office" },
            new Device { Name = "ThinkPad X1 Carbon", Manufacturer = "LENOVO", Type = "Laptop", Os = "Windows", OsVersion = "11", Processor = "Intel Core i7", RamAmount = "16GB", Location = "Shelf C1" },
            new Device { Name = "HP Spectre x360", Manufacturer = "HP", Type = "Laptop", Os = "Windows", OsVersion = "11", Processor = "Intel Core Ultra 7", RamAmount = "16GB", Location = "HQ - Management" },
            new Device { Name = "Razer Blade 16", Manufacturer = "RAZER", Type = "Laptop", Os = "Windows", OsVersion = "11", Processor = "Intel Core i9-14900HX", RamAmount = "32GB", Location = "Gaming Lounge" },
            new Device { Name = "ThinkPad T14", Manufacturer = "LENOVO", Type = "Laptop", Os = "Windows", OsVersion = "11", Processor = "AMD Ryzen 7 PRO", RamAmount = "32GB", Location = "HQ - Dev Dept" },
            new Device { Name = "ZenBook 14 OLED", Manufacturer = "ASUS", Type = "Laptop", Os = "Windows", OsVersion = "11", Processor = "Intel Core Ultra 7", RamAmount = "16GB", Location = "Tech Lab" },

            // Handhelds & Others
            new Device { Name = "ASUS ROG Ally X", Manufacturer = "ASUS", Type = "Handheld", Os = "Windows", OsVersion = "11", Processor = "Ryzen Z1 Extreme", RamAmount = "24GB", Location = "Gaming Lounge" },
            new Device { Name = "Steam Deck OLED", Manufacturer = "VALVE", Type = "Handheld", Os = "SteamOS", OsVersion = "3", Processor = "6 nm AMD APU", RamAmount = "16GB", Location = "Gaming Lounge" },
            new Device { Name = "Legion Go", Manufacturer = "LENOVO", Type = "Handheld", Os = "Windows", OsVersion = "11", Processor = "Ryzen Z1 Extreme", RamAmount = "16GB", Location = "Gaming Lounge" }
        };

        if (users.Any())
        {
            for (int i = 0; i < 15; i++)
            {
                Random rand = new Random();
                var user = users[rand.Next(users.Count)];
                var device = devices[rand.Next(devices.Count)];
                if (device.AssignedUserId != null)
                {
                    i--;
                    continue;
                }
                device.AssignedUserId = user.Id;
            }
        }

        await _context.Devices.InsertManyAsync(devices);
    }
}
