using Microsoft.AspNetCore.Mvc; // for creating controller actions
using Microsoft.AspNetCore.Mvc.RazorPages; // for Razor Page model

namespace MossPlatform.Pages // defining the namespace
{
   public class LogoutModel : PageModel // creating a Razor Page model
   {
       // OnPost action to handle the HTTP POST request for the logout functionality
       public IActionResult OnPost()
       {
           // Removing the session variable 'IsAdminLoggedIn' to logout the user
           HttpContext.Session.Remove("IsAdminLoggedIn");

           // Redirecting the user to the index page after logout
           return RedirectToPage("/Index");
       }
   }
}