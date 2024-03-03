using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MossPlatform.Models;

namespace MossPlatform.Pages
{
    public class UploadModel : PageModel
    {
        private readonly GameDataService _gameDataService;
        private readonly ImageService _imageService;
        private readonly ILogger<UploadModel> _logger;

        public UploadModel(GameDataService gameDataService, ImageService imageService, ILogger<UploadModel> logger)
        {
            _gameDataService = gameDataService;
            _imageService = imageService;
            _logger = logger;
        }

        [BindProperty]
        public Game NewGame { get; set; } = new Game();

        [BindProperty]
        public IFormFile UploadedImage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadedImage != null && UploadedImage.Length > 0)
            {
                // Utilize your existing ImageService to save the uploaded image and get the relative path
                NewGame.ImageUrl = await _imageService.SaveImageAsync(UploadedImage, NewGame.Title);
            }
            else
            {
                // Handle the case where no image is uploaded, if necessary
                _logger.LogWarning("No image uploaded for game: {Title}", NewGame.Title);
            }

            // Assuming NewGame already has Title, Description, Category, and PlayUrl set from form data due to [BindProperty]
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
                // Optionally return to the page with an error message
                ModelState.AddModelError(string.Empty, "An error occurred saving the game");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
