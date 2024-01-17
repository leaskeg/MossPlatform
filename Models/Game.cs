namespace MossPlatform.Models
{
    public class Game
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

    }
}
