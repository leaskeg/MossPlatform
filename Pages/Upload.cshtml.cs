using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MossPlatform.Models;
using System.Text.Json;

namespace MossPlatform.Pages
{
    public class UploadModel : PageModel
    {

        private readonly IWebHostEnvironment _env; // Declare the private field

        public UploadModel(IWebHostEnvironment env) // Inject through constructor
        {
            _env = env; // Assign to the private field
        }

        [BindProperty]
        public Game NewGame { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Set the Category property based on the form input
            NewGame.Category = Request.Form["Category"].ToString();

            var filePath = Path.Combine(_env.ContentRootPath, "path_to_your_games.json");

            List<Game> games;
            // Read the existing games
            if (System.IO.File.Exists(filePath))
            {
                var jsonData = await System.IO.File.ReadAllTextAsync(filePath);
                games = JsonSerializer.Deserialize<List<Game>>(jsonData) ?? new List<Game>();
            }
            else
            {
                games = new List<Game>();
            }

            // Add the new game
            games.Add(NewGame); // NewGame already has an Id due to the default in the model

            // Serialize and write the updated list to the file
            var updatedJsonData = JsonSerializer.Serialize(games, new JsonSerializerOptions { WriteIndented = true });
            await System.IO.File.WriteAllTextAsync(filePath, updatedJsonData);

            return RedirectToPage("/Index");
        }


    }

}
