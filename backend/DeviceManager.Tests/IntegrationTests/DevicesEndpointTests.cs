using System.Net;
using System.Net.Http.Json;
using DeviceManager.Core.DTOs;
using FluentAssertions;
using Xunit;

namespace DeviceManager.Tests.IntegrationTests;

[Collection("Integration Tests")]
public class DevicesEndpointTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public DevicesEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    public Task InitializeAsync()
    {
        // Dropping the entire test database to ensure a clean state before every single test
        var client = new MongoDB.Driver.MongoClient(_factory.MongoDbConnectionString);
        return client.DropDatabaseAsync("IntegrationTestDB");
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetDevices_WhenDatabaseIsEmpty_ShouldReturnEmptyList()
    {
        // Act
        var response = await _client.GetAsync("/api/devices");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var devices = await response.Content.ReadFromJsonAsync<List<DeviceDto>>();
        devices.Should().NotBeNull();
        devices.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateDevice_WhenValidData_ShouldReturnCreatedAndBeRetrievable()
    {
        // Arrange
        var newDevice = new CreateDeviceDto
        {
            Name = "Galaxy S24",
            Manufacturer = "Samsung",
            Type = "phone",
            Os = "Android",
            OsVersion = "14.0",
            Processor = "Snapdragon 8 Gen 3",
            RamAmount = "8GB",
            Description = "Corporate phone"
        };

        // Act - Create
        var createResponse = await _client.PostAsJsonAsync("/api/devices", newDevice);

        // Assert - Create
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdDevice = await createResponse.Content.ReadFromJsonAsync<DeviceDto>();
        createdDevice.Should().NotBeNull();
        createdDevice!.Id.Should().NotBeNullOrEmpty();
        createdDevice.Name.Should().Be(newDevice.Name);

        // Act - Get By Id
        var getResponse = await _client.GetAsync($"/api/devices/{createdDevice.Id}");

        // Assert - Get By Id
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var retrievedDevice = await getResponse.Content.ReadFromJsonAsync<DeviceDto>();
        retrievedDevice.Should().NotBeNull();
        retrievedDevice!.Id.Should().Be(createdDevice.Id);
        retrievedDevice.Name.Should().Be(newDevice.Name);
    }

    [Fact]
    public async Task CreateDevice_WhenDeviceAlreadyExists_ShouldReturnConflict()
    {
        // Arrange
        var newDevice = new CreateDeviceDto
        {
            Name = "iPhone 15",
            Manufacturer = "Apple",
            Type = "phone",
            Os = "iOS",
            OsVersion = "17.0",
            Processor = "A16",
            RamAmount = "6GB"
        };

        // Act - First creation
        await _client.PostAsJsonAsync("/api/devices", newDevice);
        
        // Act - Duplicate creation attempt
        var duplicateResponse = await _client.PostAsJsonAsync("/api/devices", newDevice);

        // Assert
        duplicateResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var error = await duplicateResponse.Content.ReadFromJsonAsync<ErrorResponse>();
        error.Should().NotBeNull();
        error!.StatusCode.Should().Be(409);
        error.Message.Should().Contain(newDevice.Name);
    }

    [Fact]
    public async Task GetDeviceById_WhenNotExists_ShouldReturnNotFound()
    {
        // Arrange
        var randomObjectId = "5f9b5b9b9b9b9b9b9b9b9b9b"; // valid 24-character hex format for MongoDB

        // Act
        var response = await _client.GetAsync($"/api/devices/{randomObjectId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        error.Should().NotBeNull();
        error!.StatusCode.Should().Be(404);
        error.Message.Should().Contain(randomObjectId);
    }
}
