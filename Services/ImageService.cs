public class ImageService
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ImageService> _logger; // Assuming you inject a logger

    public ImageService(IWebHostEnvironment env, ILogger<ImageService> logger)
    {
        _env = env;
        _logger = logger;
    }

    public async Task<string> SaveImageAsync(IFormFile uploadedImage, string title)
    {
        // Validate file size and type
        const long maxFileSize = 5_000_000; // 5 MB
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(uploadedImage.FileName).ToLower();

        if (!allowedExtensions.Contains(fileExtension))
        {
            throw new InvalidOperationException("Unsupported file type.");
        }

        if (uploadedImage.Length > maxFileSize)
        {
            throw new InvalidOperationException("File size exceeds the limit.");
        }

        var sanitizedTitle = SanitizeFileName(title);
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var fileName = $"{sanitizedTitle}_{timestamp}{fileExtension}";
        var directoryPath = Path.Combine(_env.WebRootPath, "uploads");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var filePath = Path.Combine(directoryPath, fileName);

        try
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await uploadedImage.CopyToAsync(fileStream);
            }
            _logger.LogInformation($"Image saved: {filePath}");
            // Return a web-friendly path
            return $"/uploads/{fileName}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save image");
            throw new InvalidOperationException($"An error occurred while saving the file: {ex.Message}", ex);
        }
    }

    private string SanitizeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var validFileName = new string(fileName.Where(ch => !invalidChars.Contains(ch)).ToArray());
        return validFileName.Replace(" ", "_");
    }
}
