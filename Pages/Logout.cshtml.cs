using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TodoApi.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Remove the AuthToken cookie
            Response.Cookies.Delete("AuthToken");
            // Optionally clear session or other user data here
            return RedirectToPage("/Login");
        }
    }
}
