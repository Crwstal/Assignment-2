using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using WebApplication3.Model;

namespace WebApplication3.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
		private readonly SignInManager<ApplicationUser> signInManager;

		public IndexModel(SignInManager<ApplicationUser> signInManager)
		{
			this.signInManager = signInManager;
		}

		public async Task<IActionResult> OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Check if the "LoggedIn" and "AuthToken" sessions are null
                var loggedIn = HttpContext.Session.GetString("LoggedIn");
                var authTokenSession = HttpContext.Session.GetString("AuthToken");

                // Check if the "AuthToken" cookie is null
                var authTokenCookie = Request.Cookies["AuthToken"];

                // Check if the session "AuthToken" is equal to the cookie "AuthToken"
                if (string.IsNullOrEmpty(loggedIn) && string.IsNullOrEmpty(authTokenSession) && authTokenSession != authTokenCookie)
				{

					ClearSessionAndCookies();

					var user = await signInManager.UserManager.GetUserAsync(User);
					if (user != null)
					{
						user.IsLoggedOn = false;
						await signInManager.UserManager.UpdateAsync(user);
					}

					await signInManager.SignOutAsync();
					return RedirectToPage("Login");
                }


            }

            return Page();
        }
		private void ClearSessionAndCookies()
		{
			HttpContext.Session.Clear();

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
