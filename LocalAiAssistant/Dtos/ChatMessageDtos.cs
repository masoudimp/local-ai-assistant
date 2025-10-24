namespace LocalAiAssistant.Dtos
{
    public class ChatMessageRequestDto
    {
        public string? Text { get; set; }
        public IFormFile? File { get; set; }
    }

}
