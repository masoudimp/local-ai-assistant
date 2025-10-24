using LocalAiAssistantService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using UglyToad.PdfPig;

namespace LocalAiAssistantService.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;

        public FileService(ILogger<FileService> logger)
        {
            _logger = logger;
        }

        public async Task<string> ExtractTextAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("⚠️ No file provided or file is empty.");
                throw new ArgumentException("No file provided or file is empty.");
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            // Define allowed extensions
            var allowedExtensions = new[] { ".pdf" };
            if (!allowedExtensions.Contains(extension))
            {
                _logger.LogWarning("❌ Unsupported file type: {Extension}. Only PDF files are allowed.", extension);
                throw new NotSupportedException($"Unsupported file format '{extension}'. Only PDF files are accepted.");
            }

            try
            {
                return await ExtractFromPdfAsync(file);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error extracting text from file: {File}", file.FileName);
                throw new InvalidOperationException($"Error extracting text from file '{file.FileName}'", ex);
            }
        }

        private async Task<string> ExtractFromPdfAsync(IFormFile file)
        {
            var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");

            await using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var sb = new StringBuilder();

            using (var pdf = PdfDocument.Open(tempPath))
            {
                foreach (var page in pdf.GetPages())
                {
                    sb.AppendLine(page.Text);
                }
            }

            try
            {
                File.Delete(tempPath);
            }
            catch (Exception deleteEx)
            {
                _logger.LogWarning(deleteEx, "⚠️ Could not delete temp file {TempPath}", tempPath);
            }

            _logger.LogInformation("✅ Extracted text from PDF: {FileName}", file.FileName);
            return sb.ToString().Trim();
        }
    }
}
