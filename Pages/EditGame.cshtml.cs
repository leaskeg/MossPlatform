using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MossPlatform.Models;
using System.Text.Json;

namespace MossPlatform.Pages
{
    public class EditGameModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _env;

        [BindProperty]
        public Game Game { get; set; }

        public EditGameModel(ILogger<IndexModel> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var filePath = Path.Combine(_env.ContentRootPath, "game-list.json");
            if (System.IO.File.Exists(filePath))
            {
                string jsonData = await System.IO.File.ReadAllTextAsync(filePath);
                var games = JsonSerializer.Deserialize<List<Game>>(jsonData) ?? new List<Game>();
                Game = games.FirstOrDefault(g => g.Id == id);
            }

            if (Game == null)
            {
                return RedirectToPage("./Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var filePath = Path.Combine(_env.ContentRootPath, "game-list.json");
            if (!System.IO.File.Exists(filePath) || Game == null)
            {
                return Page();
            }

            string jsonData = await System.IO.File.ReadAllTextAsync(filePath);
            var games = JsonSerializer.Deserialize<List<Game>>(jsonData) ?? new List<Game>();

            var gameIndex = games.FindIndex(g => g.Id == Game.Id);
            if (gameIndex != -1)
            {
                games[gameIndex] = Game;
                string updatedJsonData = JsonSerializer.Serialize(games, new JsonSerializerOptions { WriteIndented = true });
                await System.IO.File.WriteAllTextAsync(filePath, updatedJsonData);
            }

            return RedirectToPage("./Index");
        }
    }
}
