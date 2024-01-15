using System.IO;
using System.Text.Json;
using System.Security.Cryptography;

namespace MossPlatform
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Register session services before building the app
            builder.Services.AddSession(options =>
            {
                // You can set session options here if needed
                options.IdleTimeout = TimeSpan.FromMinutes(30); // for example
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Build the app
            var app = builder.Build();

            // Use session middleware after building the app
            app.UseSession();

            // Initialize admin credentials if they don't exist
            await EnsureAdminCredentials(app.Environment.ContentRootPath);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Authentication and Authorization middleware would be here
            // Since you're not using ASP.NET Core Identity, you would handle auth in your custom code

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        private static async Task EnsureAdminCredentials(string contentRootPath)
        {
            string adminCredentialsPath = Path.Combine(contentRootPath, "admin_credentials.json");
            if (!File.Exists(adminCredentialsPath))
            {
                // You should change this to your desired default username and a secure password
                string defaultUsername = "admin";
                string defaultPassword = "securepassword"; // TODO: Use a more secure password!

                // Hash the default password (you should use a proper hashing algorithm like BCrypt)
                using var sha256 = SHA256.Create();
                var hashedPasswordBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(defaultPassword));
                string hashedPassword = BitConverter.ToString(hashedPasswordBytes).Replace("-", "").ToLowerInvariant();

                var adminCredentials = new { Username = defaultUsername, PasswordHash = hashedPassword };
                string json = JsonSerializer.Serialize(adminCredentials, new JsonSerializerOptions { WriteIndented = true });

                await File.WriteAllTextAsync(adminCredentialsPath, json);
            }
        }
    }
}
