using Assignment_2.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace Assignment_2.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Login LModel { get; set; }
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IHttpContextAccessor contxt;

        public LoginModel(SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            this.signInManager = signInManager;
            contxt = httpContextAccessor;
        }

        public string LockoutMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await signInManager.UserManager.FindByEmailAsync(LModel.Email);

                if (user != null)
                {
                    // Check if the provided session identifier matches the stored one
                     if (user.IsLoggedOn)
                    {
                        LockoutMessage = "User is already logged in.";
                        return Page();
                    }

                    var result = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password,
                        LModel.RememberMe, lockoutOnFailure: true);

                    if (result.Succeeded)
                    {
                        // Set email in session
                        HttpContext.Session.SetString("LoggedIn", LModel.Email);

                        // Generate a unique identifier (guid) for AuthToken
                        string guid = Guid.NewGuid().ToString();

                        // Set AuthToken in session
                        HttpContext.Session.SetString("AuthToken", guid);

                        // Set AuthToken as a cookie
                        Response.Cookies.Append("AuthToken", guid, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true, // Make sure to set this to true in a production environment if using HTTPS
                            SameSite = SameSiteMode.None // Adjust based on your application's requirements
                        });

                        // Update LastLogin
                        TimeZoneInfo SST = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
                        user.LastLogin = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, SST);
                        user.IsLoggedOn = true;// Update session identifier
                        await signInManager.UserManager.UpdateAsync(user);

                        return RedirectToPage("Index");
                    }

                    if (result.IsLockedOut)
                    {
                        LockoutMessage = "Account is locked out. Please try again later.";
                        return Page();
                    }
                    else
                    {
                        LockoutMessage = "Email or Password incorrect";
                    }
                }
            }

            return Page();
        }
    }
}
