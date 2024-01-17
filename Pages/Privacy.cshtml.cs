using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MossPlatform.Pages
{
    public class PrivacyModel : PageModel
    {
        public string? GameUrl { get; set; }

        public void OnGet()
        {
            GameUrl = Request.Query["gameUrl"];
        }
    }
}