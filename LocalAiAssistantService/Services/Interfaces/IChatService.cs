using Microsoft.AspNetCore.Http;

namespace LocalAiAssistantService.Services.Interfaces
{
    public interface IChatService
    {
        Task<string> SendMessageAsync(string? text, IFormFile? file);
    }
}
