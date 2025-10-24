using LocalAiAssistant.Dtos;
using LocalAiAssistantService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocalAiAssistant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ILogger<ChatController> _logger;
        private readonly IChatService _chatService;

        // Dependency injection for both logger and service
        public ChatController(ILogger<ChatController> logger, IChatService chatService)
        {
            _logger = logger;
            _chatService = chatService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromForm] ChatMessageRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Text) && request.File == null)
                return BadRequest("You must provide either text or a file.");

            try
            {
                // Delegate logic to your ChatService
                var response = await _chatService.SendMessageAsync(request.Text, request.File);

                // Log and return
                _logger.LogInformation("Chat request processed successfully.");
                return Ok(new { success = true, response });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing chat message.");
                return StatusCode(500, new { success = false, error = "Internal server error." });
            }
        }
    }
}
