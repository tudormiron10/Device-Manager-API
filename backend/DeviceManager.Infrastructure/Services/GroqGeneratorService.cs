using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using DeviceManager.Core.DTOs;
using DeviceManager.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DeviceManager.Infrastructure.Services;

/// <summary>
/// Implementation of IGeneratorService using Groq API (OpenAI compatible).
/// Generates professional hardware descriptions using high-speed Llama 3 models.
/// </summary>
public class GroqGeneratorService : IGeneratorService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GroqGeneratorService> _logger;

    public GroqGeneratorService(HttpClient httpClient, IConfiguration configuration, ILogger<GroqGeneratorService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> GenerateDeviceDescriptionAsync(DeviceGeneratorDto specs)
    {
        var settings = _configuration.GetSection("GroqSettings");
        var apiKey = settings["ApiKey"];
        var baseUrl = settings["BaseUrl"];
        var model = settings["Model"];

        if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_GROQ_API_KEY_HERE" || apiKey == "[GROQ_API_KEY]")
        {
            return $"A premium {specs.Manufacturer} {specs.Type} designed for industrial use. Features {specs.RamAmount} RAM and {specs.Os} {specs.OsVersion}, providing a robust solution for logistics operations.";
        }

        try
        {
            var prompt = $@"Write a concise, professional, and readable description (MAX 2 SENTENCES) for a hardware device with these specifications:
                - Name: {specs.Name}
                - Manufacturer: {specs.Manufacturer}
                - Type: {specs.Type}
                - OS: {specs.Os} {specs.OsVersion}
                - Processor: {specs.Processor}
                - RAM: {specs.RamAmount}
                
                The description should be suitable for an industrial inventory system. Do not use emojis.";

            var requestBody = new
            {
                model = model,
                messages = new[]
                {
                    new { role = "system", content = "You are a professional industrial hardware documentation specialist." },
                    new { role = "user", content = prompt }
                },
                temperature = 0.5,
                max_tokens = 150
            };

            var url = $"{baseUrl}/chat/completions";
            
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("Authorization", $"Bearer {apiKey}");
            request.Content = JsonContent.Create(requestBody);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError("Groq API Error: {Status} - {Error}", response.StatusCode, error);
                return FallbackDescription(specs);
            }

            var result = await response.Content.ReadFromJsonAsync<GroqChatResponse>();
            var generatedText = result?.Choices?[0]?.Message?.Content?.Trim();

            return !string.IsNullOrEmpty(generatedText) ? generatedText : FallbackDescription(specs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception during Groq generation");
            return FallbackDescription(specs);
        }
    }

    private static string FallbackDescription(DeviceGeneratorDto specs)
    {
        return $"High-performance {specs.Manufacturer} {specs.Type} integrated with {specs.Os} {specs.OsVersion}. Features an {specs.Processor} CPU and {specs.RamAmount} RAM, configured for MarshTech logistics operations.";
    }

    private class GroqChatResponse
    {
        [JsonPropertyName("choices")]
        public Choice[]? Choices { get; set; }
    }

    private class Choice
    {
        [JsonPropertyName("message")]
        public Message? Message { get; set; }
    }

    private class Message
    {
        [JsonPropertyName("content")]
        public string? Content { get; set; }
    }
}
