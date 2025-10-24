using Microsoft.AspNetCore.Http;

namespace LocalAiAssistantService.Services.Interfaces
{
    /// <summary>
    /// Defines a contract for sending generic model requests and receiving typed responses.
    /// </summary>
    public interface IModelBridgeService
    {
        /// <summary>
        /// Sends a model request payload and returns a typed response.
        /// </summary>
        /// <typeparam name="T">The expected response DTO type.</typeparam>
        /// <param name="payload">The payload to send to the model backend.</param>
        /// <param name="file">Optional file input for the request.</param>
        Task<T> SendToOModelAsync<T>(object payload, IFormFile? file);
    }
}
