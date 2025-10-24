using LocalAiAssistantService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LocalAiAssistantService.Services.Implementations
{
    public class ChatService : IChatService
    {
        private readonly ILogger<ChatService> _logger;
        private readonly IModelService _modelService;
        private readonly IFileService _fileService;

        public ChatService(ILogger<ChatService> logger, IModelService modelService, IFileService fileService)
        {
            _logger = logger;
            _modelService = modelService;
            _fileService = fileService;
        }

        public async Task<string> SendMessageAsync(string? text, IFormFile? file)
        {
            string inputText = text ?? "";

            if (file != null)
            {
                var extracted = await _fileService.ExtractTextAsync(file);
                inputText += "\n\nExtracted from file:\n" + extracted;
            }

            var response = await _modelService.SendToOModelAsync(inputText, file);
            return response.Response ?? "No response received.";
        }
    }
}
