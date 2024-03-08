using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MossPlatform.Models;

namespace MossPlatform.Pages
{
    public class UploadModel : PageModel
    {
        // Inject services for game data management, image processing, and logging
        private readonly GameDataService _gameDataService;
        private readonly ImageService _imageService;
        private readonly ILogger<UploadModel> _logger;

        // Constructor initializes services through Dependency Injection
        public UploadModel(GameDataService gameDataService, ImageService imageService, ILogger<UploadModel> logger)
        {
            _gameDataService = gameDataService;
            _imageService = imageService;
            _logger = logger;
        }

        // Properties bound to the form input, enabling model binding for game information and uploaded image
        [BindProperty]
        public Game NewGame { get; set; } = new Game();

        [BindProperty]
        public IFormFile UploadedImage { get; set; }

        // Handles the form submission (POST request)
        public async Task<IActionResult> OnPostAsync()
        {
            // Define a list of allowed image formats for validation
            var allowedImageFormats = new List<string> { "image/jpeg", "image/png", "image/gif" };

            // Check if an image was uploaded
            if (UploadedImage != null && UploadedImage.Length > 0)
            {
                // Check if the image is in one of the allowed formats
                if (!allowedImageFormats.Contains(UploadedImage.ContentType))
                {
                    // Log a warning and add a model error for an unsupported image format
                    _logger.LogWarning("Unsupported image format uploaded for game: {Title}", NewGame.Title);
                    ModelState.AddModelError(string.Empty, "Unsupported image format. Please upload a JPEG, PNG, or GIF.");
                    return Page(); // Return to the page to display the error message
                }

                // Save the uploaded image using the ImageService and update the game's ImageUrl
                NewGame.ImageUrl = await _imageService.SaveImageAsync(UploadedImage, NewGame.Title);
            }
            else
            {
                // Log a warning if no image was uploaded
                _logger.LogWarning("No image uploaded for game: {Title}", NewGame.Title);
            }

            // Continue with the rest of the method (saving the game, handling other errors, etc.)
            try
            {
                var games = await _gameDataService.GetGamesAsync();
                games.Add(NewGame);
                await _gameDataService.SaveGamesAsync(games);
                _logger.LogInformation("New game added: {Title}", NewGame.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding game: {Title}", NewGame.Title);
                ModelState.AddModelError(string.Empty, "An error occurred saving the game");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
