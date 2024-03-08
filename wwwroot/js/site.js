// Load a game into an iframe
function loadGame(gameUrl) {
    var iframe = document.getElementById('gameFrame');
    var container = document.getElementById('gameContainer');

    iframe.src = gameUrl;
    container.style.display = 'block';
}

// Function to initialize or apply the dark/light theme
function applyTheme(theme) {
    const bodyClassList = document.body.classList;
    const logoImage = document.getElementById('logo-image');
    const lightModeIcon = document.getElementById('light-mode-icon');
    const darkModeIcon = document.getElementById('dark-mode-icon');
    const DARK_MODE = 'dark-mode';
    const LIGHT_MODE = 'light-mode';

    const isDarkMode = theme === DARK_MODE;
    bodyClassList.add(isDarkMode ? DARK_MODE : LIGHT_MODE);
    bodyClassList.remove(isDarkMode ? LIGHT_MODE : DARK_MODE);

    lightModeIcon.style.display = isDarkMode ? 'block' : 'none';
    darkModeIcon.style.display = isDarkMode ? 'none' : 'block';

    logoImage.src = isDarkMode ? '/css/Resources/Images/DORFhvid.png' : '/css/Resources/Images/DORFgrøn.png';
    localStorage.setItem('theme', theme);
}

// Function to handle theme switching
function setupThemeToggleButton() {
    const themeToggleButton = document.getElementById('theme-toggle');
    const currentTheme = localStorage.getItem('theme') || 'light-mode'; // Default to light mode if no preference is saved

    applyTheme(currentTheme); // Apply the initial theme

    themeToggleButton.addEventListener('click', () => {
        const newTheme = document.body.classList.contains('dark-mode') ? 'light-mode' : 'dark-mode';
        applyTheme(newTheme);
    });
}

// Set up everything after the DOM has loaded
document.addEventListener('DOMContentLoaded', (event) => {
    setupThemeToggleButton();
    document.body.style.display = ''; // Show the body after everything is set up
});

