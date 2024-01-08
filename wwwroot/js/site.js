function loadGame(gameUrl) {
    // Get the iframe element and the container
    var iframe = document.getElementById('gameFrame');
    var container = document.getElementById('gameContainer');

    // Set the game URL as the src of the iframe
    iframe.src = gameUrl;

    // Show the container
    container.style.display = 'block';
}
