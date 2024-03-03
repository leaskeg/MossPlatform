namespace MossPlatform.Models
{
    public enum GameCategory
    {
        Featured,
        New,
        // other categories
    }
    public class Game
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; } // Original string category
        public GameCategory CategoryEnum { get; set; } // New enum property for internal use
        public string ImageUrl { get; set; }
        public string PlayUrl { get; set; }
    }

}
