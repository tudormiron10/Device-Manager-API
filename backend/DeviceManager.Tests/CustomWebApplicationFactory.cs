using DeviceManager.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Testcontainers.MongoDb;

namespace DeviceManager.Tests;

/// <summary>
/// Custom factory for setting up a Testcontainer Mongo DB running implicitly for the integration tests.
/// We hook up the actual backend logic with a fake in-memory / containerized connection.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer;

    public CustomWebApplicationFactory()
    {
        _mongoDbContainer = new MongoDbBuilder()
            .WithImage("mongo:7")
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Configure the MongoDB connection string to point to the newly spun up Testcontainer
            // We use PostConfigure to easily override the settings loaded from appsettings.json
            services.PostConfigure<MongoDbSettings>(settings =>
            {
                settings.ConnectionString = _mongoDbContainer.GetConnectionString();
                settings.DatabaseName = "IntegrationTestDB";
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
    }

    public string MongoDbConnectionString => _mongoDbContainer.GetConnectionString();

    public new async Task DisposeAsync()
    {
        await _mongoDbContainer.DisposeAsync().AsTask();
    }
}
