using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text.Json;

namespace MossPlatform.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginInputModel Input { get; set; }

        public string Message { get; set; }

        public class LoginInputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string jsonCredentials = await System.IO.File.ReadAllTextAsync("admin_credentials.json");
            var adminCredentials = JsonSerializer.Deserialize<AdminCredentials>(jsonCredentials);

            // Ensure adminCredentials is not null
            if (adminCredentials == null)
            {
                Message = "Server error. Please try again later.";
                return Page(); // You might want to log this condition as well.
            }

            using var sha256 = SHA256.Create();
            var hashedPasswordBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Input.Password));
            string hashedPassword = BitConverter.ToString(hashedPasswordBytes).Replace("-", "").ToLowerInvariant();

            if (adminCredentials.Username == Input.Username && adminCredentials.PasswordHash == hashedPassword)
            {
                HttpContext.Session.SetString("IsAdminLoggedIn", "true"); // Consider using standard authentication mechanisms instead
                return RedirectToPage("/Index");
            }
            else
            {
                // Authentication failed
                Message = "Invalid login attempt.";
                return Page();
            }
        }

        private class AdminCredentials
        {
            public string Username { get; set; }
            public string PasswordHash { get; set; }
        }
    }
}