using LocalAiAssistantService.Dtos.Models;
using LocalAiAssistantService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LocalAiAssistantService.Services.Implementations
{
    public class GemmiModelService : IModelService
    {
        private readonly IModelBridgeService _modelBridgeService;
        private readonly ILogger<GemmiModelService> _logger;

        public GemmiModelService(IModelBridgeService bridge, ILogger<GemmiModelService> logger)
        {
            _modelBridgeService = bridge;
            _logger = logger;
        }

        public async Task<GemminiResponseDto> SendToOModelAsync(string prompt, IFormFile? file)
        {
            if (!ValidatePrompt(prompt, out var validationError)) return validationError;

            var payload = BuildPayload(prompt);
            _logger.LogInformation("🔁 Routing prompt through GemmiModelService with model {Model}", payload.Model);

            return await ExecuteModelRequestAsync(payload, file);
        }

        #region Private Helpers

        private bool ValidatePrompt(string prompt, out GemminiResponseDto errorResponse)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                errorResponse = new GemminiResponseDto
                {
                    Success = false,
                    Error = "⚠️ No prompt provided."
                };
                return false;
            }

            errorResponse = null!;
            return true;
        }

        private GemminiRequestDto BuildPayload(string prompt)
        {
            return new GemminiRequestDto
            {
                Model = "gemma:2b",
                Prompt = prompt
            };
        }

        private async Task<GemminiResponseDto> ExecuteModelRequestAsync(GemminiRequestDto payload, IFormFile? file)
        {
            try
            {
                var result = await _modelBridgeService.SendToOModelAsync<GemminiResponseDto>(payload, file);

                if (result == null)
                {
                    _logger.LogWarning("⚠️ Model returned null result for prompt: {Prompt}", payload.Prompt);
                    return new GemminiResponseDto
                    {
                        Success = false,
                        Error = "No valid response from model."
                    };
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error during model request execution for prompt: {Prompt}", payload.Prompt);
                return new GemminiResponseDto
                {
                    Success = false,
                    Error = "Unexpected error while communicating with model."
                };
            }
        }

        #endregion

    }
}
