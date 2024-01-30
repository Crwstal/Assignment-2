using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using WebApplication3.Model;

namespace WebApplication3.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDataProtectionProvider _dataProtectionProvider; 
        public IndexModel(ILogger<IndexModel> logger, UserManager<ApplicationUser> userManager, IDataProtectionProvider dataProtectionProvider, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _dataProtectionProvider = dataProtectionProvider;
            _signInManager = signInManager;
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
                if (!string.IsNullOrEmpty(loggedIn) && !string.IsNullOrEmpty(authTokenSession) && authTokenSession == authTokenCookie)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user != null)
                    {
                        var protector = _dataProtectionProvider.CreateProtector("MySecretKey");

                        if (!string.IsNullOrEmpty(user.CreditCardNo))
                        {
                            try
                            {
                                var decryptedCreditCard = protector.Unprotect(user.CreditCardNo);

                                ViewData["DecryptedCreditCard"] = decryptedCreditCard;
                            }
                            catch (CryptographicException ex)
                            {
                                _logger.LogError(ex, "Error decrypting CreditCardNo");
                                ViewData["DecryptedCreditCard"] = "Error decrypting CreditCardNo";
                            }
                        }
                    }
                }
                else
                {
                    ViewData["DecryptedCreditCard"] = "User is not logged in";
                    return RedirectToPage("Login");
                }
            }

            return Page();
        }

    }
}
