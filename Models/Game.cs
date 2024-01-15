namespace MossPlatform.Models
{
    public class Game
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Category { get; set; }
    }
}
