function loadGame(gameUrl) {
    // Get the iframe element and the container
    var iframe = document.getElementById('gameFrame');
    var container = document.getElementById('gameContainer');

    // Set the game URL as the src of the iframe
    iframe.src = gameUrl;

    // Show the container
    container.style.display = 'block';
}

// Dark mode section

document.addEventListener('DOMContentLoaded', (event) => {
    const themeToggleButton = document.getElementById('theme-toggle');
    const logoImage = document.getElementById('logo-image'); // Get the logo image element
    const lightModeIcon = document.getElementById('light-mode-icon');
    const darkModeIcon = document.getElementById('dark-mode-icon');
    const currentTheme = localStorage.getItem('theme') || 'light'; // Default to light mode

    // Function to update logo based on theme
    function updateLogo(theme) {
        if (theme === 'dark') {
            logoImage.src = '/css/Resources/Images/DORFhvid.png'; // Update this path
        } else {
            logoImage.src = '/css/Resources/Images/DORFgrøn.png'; // Update this path
        }
    }

    // Apply the initial theme, icon, and logo
    if (currentTheme === 'dark') {
        document.body.classList.add('dark-mode');
        lightModeIcon.style.display = 'block';
        darkModeIcon.style.display = 'none';
        updateLogo('dark');
    } else {
        document.body.classList.add('light-mode');
        lightModeIcon.style.display = 'none';
        darkModeIcon.style.display = 'block';
        updateLogo('light');
    }

    themeToggleButton.addEventListener('click', () => {
        if (document.body.classList.contains('dark-mode')) {
            document.body.classList.replace('dark-mode', 'light-mode');
            lightModeIcon.style.display = 'none';
            darkModeIcon.style.display = 'block';
            updateLogo('light');
            localStorage.setItem('theme', 'light');
        } else {
            document.body.classList.replace('light-mode', 'dark-mode');
            lightModeIcon.style.display = 'block';
            darkModeIcon.style.display = 'none';
            updateLogo('dark');
            localStorage.setItem('theme', 'dark');
        }
    });
});


