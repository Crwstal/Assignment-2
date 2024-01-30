using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Assignment_2.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;

        public LogoutModel(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            ClearSessionAndCookies();

            // Update IsLoggedOn to false during logout
            var user = await signInManager.UserManager.GetUserAsync(User);
            if (user != null)
            {
                user.IsLoggedOn = false;
                await signInManager.UserManager.UpdateAsync(user);
            }

            await signInManager.SignOutAsync();
            return RedirectToPage("Login");
        }

        public IActionResult OnPostDontLogout()
        {
            return RedirectToPage("Index");
        }

        private void ClearSessionAndCookies()
        {
            HttpContext.Session.Clear();

            // Adjust the cookie names based on your application's configuration
            if (Request.Cookies[".AspNetCore.Session"] != null)
            {
                Response.Cookies.Delete(".AspNetCore.Session");
            }

            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies.Delete("AuthToken");
            }
        }
    }
}
