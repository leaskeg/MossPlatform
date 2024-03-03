using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MossPlatform.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MossPlatform.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _env;

        public List<Game>? Games { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async Task OnGetAsync()
        {
            var filePath = Path.Combine(_env.ContentRootPath, "path_to_your_games.json");
            if (System.IO.File.Exists(filePath))
            {
                var jsonData = await System.IO.File.ReadAllTextAsync(filePath);

                // Add JsonSerializerOptions with JsonStringEnumConverter
                var options = new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() }
                };
                var gamesList = JsonSerializer.Deserialize<List<Game>>(jsonData, options);

                Games = JsonSerializer.Deserialize<List<Game>>(jsonData, options) ?? new List<Game>();
                _logger.LogInformation("Loaded {GameCount} games.", Games?.Count ?? 0);
            }
            else
            {
                _logger.LogWarning("The games file was not found.");
                Games = new List<Game>();
            }
        }

        public async Task<IActionResult> OnPostDeleteGameAsync(string id)
        {
            var filePath = Path.Combine(_env.ContentRootPath, "path_to_your_games.json");
            if (System.IO.File.Exists(filePath))
            {
                var jsonData = await System.IO.File.ReadAllTextAsync(filePath);
                var games = JsonSerializer.Deserialize<List<Game>>(jsonData) ?? new List<Game>();

                var gameToRemove = games.FirstOrDefault(g => g.Id == id);
                if (gameToRemove != null)
                {
                    games.Remove(gameToRemove);
                    jsonData = JsonSerializer.Serialize(games, new JsonSerializerOptions { WriteIndented = true });
                    await System.IO.File.WriteAllTextAsync(filePath, jsonData);
                    _logger.LogInformation("Game with ID {GameId} was deleted.", id);
                }
                else
                {
                    _logger.LogWarning("Game with ID {GameId} was not found and could not be deleted.", id);
                }
            }
            else
            {
                _logger.LogWarning("The games file was not found, deletion could not be performed.");
            }

            return RedirectToPage();
        }

    }
}
