using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

public class ImageService
{
    // Private fields holding the environment and logger instances.
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ImageService> _logger;

    // Constructor initializes the service with the web hosting environment and logging interface.
    public ImageService(IWebHostEnvironment env, ILogger<ImageService> logger)
    {
        // Argument validation to prevent null values.
        _env = env ?? throw new ArgumentNullException(nameof(env));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // Asynchronous method to save an uploaded image file.
    public async Task<string> SaveImageAsync(IFormFile uploadedImage, string title)
    {
        // Validate input parameters to ensure they are not null or invalid.
        if (uploadedImage == null) throw new ArgumentNullException(nameof(uploadedImage));
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be null or whitespace.", nameof(title));

        try
        {
            // Define constraints and prepare file information.
            const long maxFileSize = 5_000_000; // Max file size set to 5 MB.
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" }; // Supported file extensions.
            var fileExtension = Path.GetExtension(uploadedImage.FileName).ToLower();

            // Check file type and size validity.
            if (!allowedExtensions.Contains(fileExtension)) throw new InvalidOperationException("Unsupported file type.");
            if (uploadedImage.Length > maxFileSize) throw new InvalidOperationException("File size exceeds the limit.");

            // Sanitize and prepare the file name.
            var sanitizedTitle = SanitizeFileName(title);
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); // Unique timestamp for the file name.
            var fileName = $"{sanitizedTitle}_{timestamp}{fileExtension}"; // Construct final file name.
            var directoryPath = Path.Combine(_env.WebRootPath, "uploads"); // Directory path for uploads.
            var filePath = Path.Combine(directoryPath, fileName); // Full file path.

            // Create the directory if it does not exist.
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            // Save the uploaded file to the server.
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await uploadedImage.CopyToAsync(fileStream);
            }

            // Log the successful save operation and return a web-friendly path.
            _logger.LogInformation($"Image saved: {filePath}");
            return $"/uploads/{fileName}";
        }
        catch (Exception ex)
        {
            // Log any exceptions that occur and rethrow them.
            _logger.LogError(ex, "Failed to save image");
            throw; // Re-throwing allows the calling method to handle the exception.
        }
    }

    // Helper method to sanitize the file name by removing invalid characters.
    private string SanitizeFileName(string fileName)
    {
        // Check if the file name is null or whitespace.
        if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("FileName cannot be null or whitespace.", nameof(fileName));

        try
        {
            // Remove invalid characters from the file name.
            var invalidChars = Path.GetInvalidFileNameChars();
            var validFileName = new string(fileName.Where(ch => !invalidChars.Contains(ch)).ToArray());
            return validFileName.Replace(" ", "_"); // Replace spaces with underscores for web compatibility.
        }
        catch (Exception ex)
        {
            // Log and rethrow exceptions related to file name sanitization.
            _logger.LogError(ex, "Failed to sanitize file name");
            throw new InvalidOperationException($"An error occurred while sanitizing the file name: {ex.Message}", ex);
        }
    }
}
