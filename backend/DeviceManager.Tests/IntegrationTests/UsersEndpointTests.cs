using System.Net;
using System.Net.Http.Json;
using DeviceManager.Core.DTOs;
using FluentAssertions;
using Xunit;

namespace DeviceManager.Tests.IntegrationTests;

[Collection("Integration Tests")]
public class UsersEndpointTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public UsersEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    public Task InitializeAsync()
    {
        var client = new MongoDB.Driver.MongoClient(_factory.MongoDbConnectionString);
        return client.DropDatabaseAsync("IntegrationTestDB");
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetUsers_WhenDatabaseIsEmpty_ShouldReturnEmptyList()
    {
        // Act
        var response = await _client.GetAsync("/api/users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
        users.Should().NotBeNull();
        users.Should().BeEmpty();
    }

    [Fact]
    public async Task GetUserById_WhenNotExists_ShouldReturnNotFound()
    {
        // Arrange
        var randomObjectId = "7a9b5b9b9b9b9b9b9b9b9b9a"; // valid 24-character hex format for MongoDB

        // Act
        var response = await _client.GetAsync($"/api/users/{randomObjectId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        error.Should().NotBeNull();
        error!.StatusCode.Should().Be(404);
        error.Message.Should().Contain(randomObjectId);
    }
}
