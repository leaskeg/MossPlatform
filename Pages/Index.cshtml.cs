using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MossPlatform.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MossPlatform.Pages
{
   public class IndexModel : PageModel // Class for the Index page model
   {
       private readonly ILogger<IndexModel> _logger;
       private readonly IWebHostEnvironment _env;

       public List<Game>? Games { get; set; } // List to store the games loaded from the JSON file

       public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment env) // Constructor
       {
           _logger = logger;
           _env = env;
       }

       public async Task OnGetAsync() // Method for GET request
       {
           var filePath = Path.Combine(_env.ContentRootPath, "game-list.json"); // Combine the content root path with the JSON file name

           if (System.IO.File.Exists(filePath)) // Check if the file exists
           {
               var jsonData = await System.IO.File.ReadAllTextAsync(filePath); // Read the JSON file

               // Add JsonSerializerOptions with JsonStringEnumConverter
               var options = new JsonSerializerOptions
               {
                   Converters = { new JsonStringEnumConverter() }
               };

               var gamesList = JsonSerializer.Deserialize<List<Game>>(jsonData, options); // Deserialize the JSON data to a list of games

               Games = JsonSerializer.Deserialize<List<Game>>(jsonData, options) ?? new List<Game>(); // Set the Games property to the deserialized list of games or a new list if it's null
               _logger.LogInformation("Loaded {GameCount} games.", Games?.Count ?? 0); // Log the number of loaded games
           }
           else
           {
               _logger.LogWarning("The games file was not found."); // Log that the games file was not found
               Games = new List<Game>(); // Set the Games property to a new list
           }
       }

       public async Task<IActionResult> OnPostDeleteGameAsync(string id) // Method for POST request when deleting a game
       {
           var filePath = Path.Combine(_env.ContentRootPath, "game-list.json"); // Combine the content root path with the JSON file name

           if (System.IO.File.Exists(filePath)) // Check if the file exists
           {
               var jsonData = await System.IO.File.ReadAllTextAsync(filePath); // Read the JSON file
               var games = JsonSerializer.Deserialize<List<Game>>(jsonData) ?? new List<Game>(); // Deserialize the JSON data to a list of games

               var gameToRemove = games.FirstOrDefault(g => g.Id == id); // Find the game to remove

               if (gameToRemove != null) // If the game was found
               {
                   games.Remove(gameToRemove); // Remove the game from the list
                   jsonData = JsonSerializer.Serialize(games, new JsonSerializerOptions { WriteIndented = true }); // Serialize the updated list of games
                   await System.IO.File.WriteAllTextAsync(filePath, jsonData); // Write the serialized list of games back to the JSON file
                   _logger.LogInformation("Game with ID {GameId} was deleted.", id); // Log that the game was deleted
               }
               else
               {
                   _logger.LogWarning("Game with ID {GameId} was not found and could not be deleted.", id); // Log that the game was not found for deletion
               }
           }
           else
           {
               _logger.LogWarning("The games file was not found, deletion could not be performed."); // Log that the games file was not found for deletion
           }

           return RedirectToPage(); // Redirect to the page after deletion
       }
   }
}