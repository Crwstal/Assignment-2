using Assignment_2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace Assignment_2.Pages
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        [BindProperty]
        public ChangePassword CPModel { get; set; }

        public ChangePasswordModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
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

                    var user = await userManager.GetUserAsync(User);
                    if (user != null)
                    {
                        user.IsLoggedOn = false;
                        await userManager.UpdateAsync(user);
                    }

                    await signInManager.SignOutAsync();
                    return RedirectToPage("Login");
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (string.IsNullOrEmpty(CPModel.CurrentPassword))
            {
                ModelState.AddModelError(nameof(CPModel.CurrentPassword), "Current Password is required.");
            }

            if (string.IsNullOrEmpty(CPModel.NewPassword))
            {
                ModelState.AddModelError(nameof(CPModel.NewPassword), "New Password is required.");
            }

            if (string.IsNullOrEmpty(CPModel.ConfirmPassword))
            {
                ModelState.AddModelError(nameof(CPModel.ConfirmPassword), "Confirm Password is required.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Use a different variable name to avoid conflict
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await userManager.ChangePasswordAsync(user, CPModel.CurrentPassword, CPModel.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await signInManager.RefreshSignInAsync(user);
            return RedirectToPage("Index");
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
