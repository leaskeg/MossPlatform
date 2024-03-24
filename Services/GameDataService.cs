using MossPlatform.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

public class GameDataService
{
    // Fields for hosting environment and file path configuration.
    private readonly IWebHostEnvironment _env;
    private readonly string _filePath;

    // Constructor to initialize the game data service with the web hosting environment.
    public GameDataService(IWebHostEnvironment env)
    {
        // Ensure the environment argument is not null to avoid runtime errors.
        _env = env ?? throw new ArgumentNullException(nameof(env));
        // Set the path to the JSON file where game data is stored.
        _filePath = Path.Combine(_env.ContentRootPath, "game-list.json");
    }

    // Asynchronous method to retrieve a list of games from the JSON file.
    public async Task<List<Game>> GetGamesAsync()
    {
        try
        {
            // Check if the game data file exists.
            if (File.Exists(_filePath))
            {
                // Read the content of the JSON file.
                var jsonData = await File.ReadAllTextAsync(_filePath);
                // Deserialize the JSON data into a list of games, returning an empty list if null.
                return JsonSerializer.Deserialize<List<Game>>(jsonData) ?? new List<Game>();
            }
            // Return an empty list if the file doesn't exist.
            return new List<Game>();
        }
        catch (Exception ex)
        {
            // Log or handle the error as appropriate.
            // For example: _logger.LogError(ex, "Failed to retrieve games.");
            // Re-throw the exception to handle it further up the call stack.
            throw new InvalidOperationException($"An error occurred while retrieving games: {ex.Message}", ex);
        }
    }

    // Asynchronous method to save a list of games to the JSON file.
    public async Task SaveGamesAsync(List<Game> games)
    {
        if (games == null) throw new ArgumentNullException(nameof(games)); // Validate the games list is not null.

        try
        {
            // Set options for JSON serialization, such as indentation for readability.
            var options = new JsonSerializerOptions { WriteIndented = true };
            // Serialize the list of games into JSON format.
            var jsonData = JsonSerializer.Serialize(games, options);
            // Write the JSON data to the file asynchronously, creating or overwriting the existing file.
            await File.WriteAllTextAsync(_filePath, jsonData);
        }
        catch (Exception ex)
        {
            // Log or handle the error as appropriate.
            // For example: _logger.LogError(ex, "Failed to save games.");
            // Re-throw the exception to maintain exception flow and enable further handling.
            throw new InvalidOperationException($"An error occurred while saving games: {ex.Message}", ex);
        }
    }
}
