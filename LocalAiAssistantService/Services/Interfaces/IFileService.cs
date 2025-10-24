using Microsoft.AspNetCore.Http;

namespace LocalAiAssistantService.Services.Interfaces
{
    public interface IFileService
    {
        /// <summary>
        /// Extracts plain text from a given file (PDF, DOCX, TXT, etc.).
        /// </summary>
        Task<string> ExtractTextAsync(IFormFile file);
    }
}
