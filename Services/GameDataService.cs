using MossPlatform.Models;
using System.Text.Json;

public class GameDataService
{
    private readonly IWebHostEnvironment _env;
    private readonly string _filePath;

    public GameDataService(IWebHostEnvironment env)
    {
        _env = env;
        _filePath = Path.Combine(_env.ContentRootPath, "game-list.json");
    }

    public async Task<List<Game>> GetGamesAsync()
    {
        if (File.Exists(_filePath))
        {
            var jsonData = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<Game>>(jsonData) ?? new List<Game>();
        }
        return new List<Game>();
    }

    public async Task SaveGamesAsync(List<Game> games)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonData = JsonSerializer.Serialize(games, options);
        await File.WriteAllTextAsync(_filePath, jsonData);
    }
}
