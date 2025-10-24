using LocalAiAssistantService.Dtos.Models;
using LocalAiAssistantService.Services.Interfaces;
using LocalAiAssistantService.Settings.ModelBridgeSettings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace LocalAiAssistantService.Services.Implementations
{
    public class OllamaBridgeService : IModelBridgeService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OllamaBridgeService> _logger;
        private readonly IOptions<OllamaSettings> _settings;

        public OllamaBridgeService(HttpClient httpClient, ILogger<OllamaBridgeService> logger, IOptions<OllamaSettings> settings)
        {
            _httpClient = httpClient;
            _logger = logger;
            _settings = settings;
        }

        public async Task<T> SendToOModelAsync<T>(object payload, IFormFile? file)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload), "⚠️ Payload cannot be null.");

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await ExecuteRequestAsync<T>(content, payload);
        }

        private async Task<T> ExecuteRequestAsync<T>(HttpContent content, object payload)
        {
            try
            {
                var baseUrl = _settings.Value.BaseUrl;
                var response = await _httpClient.PostAsync(baseUrl, content);
                response.EnsureSuccessStatusCode();

                var resultJson = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("✅ Ollama raw response: {Response}", resultJson);

                // Split streaming chunks into lines, then find the last complete JSON
                var jsonObjects = resultJson
                    .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => line.Trim())
                    .Where(line => line.StartsWith("{") && line.EndsWith("}"))
                    .ToList();

                // Combine all responses into one big string (Ollama sends partials)
                var aggregatedResponse = new StringBuilder();

                foreach (var jsonLine in jsonObjects)
                {
                    try
                    {
                        using var doc = JsonDocument.Parse(jsonLine);
                        if (doc.RootElement.TryGetProperty("response", out var chunk))
                        {
                            aggregatedResponse.Append(chunk.GetString());
                        }
                    }
                    catch
                    {
                        // Skip malformed partials
                    }
                }

                // Wrap it into your DTO
                var result = Activator.CreateInstance<T>();

                if (result is GemminiResponseDto geminiDto)
                {
                    geminiDto.Success = true;
                    geminiDto.Response = aggregatedResponse.ToString();
                    return (T)(object)geminiDto;
                }

                throw new InvalidOperationException("Unsupported response DTO type");
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "❌ Network error while contacting Ollama backend.");
                throw new InvalidOperationException("Error: Unable to reach Ollama backend.", httpEx);
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "❌ Failed to parse Ollama response JSON.");
                throw new InvalidOperationException("Error: Invalid JSON returned by Ollama.", jsonEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Unexpected error while sending payload to Ollama.");
                throw;
            }
        }

    }
}
