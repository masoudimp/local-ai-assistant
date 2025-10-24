using LocalAiAssistantService.Dtos.Models;
using Microsoft.AspNetCore.Http;

namespace LocalAiAssistantService.Services.Interfaces
{
    public interface IModelService
    {
        Task<GemminiResponseDto> SendToOModelAsync(string prompt, IFormFile? file);
    }
}
