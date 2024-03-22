namespace MossPlatform.Models
{
    // Enum representing different categories a game can belong to.
    public enum GameCategory
    {
        Featured, // Represents prominently featured games.
        New,      // Represents newly added games.
        // other categories can be added here as needed.
    }

    // Class representing the data structure for a game.
    public class Game
    {
        // Unique identifier for the game, automatically generated to ensure uniqueness.
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Title of the game. Should not be null or empty.
        public string Title { get; set; }

        // Description of the game. Can be null or empty, but should be provided for better user understanding.
        public string Description { get; set; }

        // The category of the game in string format. This is for external use where enum types are not applicable.
        public string Category { get; set; }

        // The category of the game, represented as an enum for internal use and validation.
        public GameCategory CategoryEnum { get; set; }

        // URL to the game's thumbnail or image. Should be a valid URL format.
        public string ImageUrl { get; set; }

        // URL to play the game. This should be a valid URL format and is essential for the game's accessibility.
        public string PlayUrl { get; set; }
    }
}
