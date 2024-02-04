using AspNetCore.ReCaptcha;
using Assignment_2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Assignment_2.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Login LModel { get; set; }
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LoginModel> logger;
        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger)
        {
            this.signInManager = signInManager;
            this.logger = logger;
        }

        public string LockoutMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {                

            

           if (ModelState.IsValid)
            {
                if (!ReCaptchaPassed(Request.Form["recaptcha"], logger))
                {
                    // ReCaptcha validation failed
                    logger.LogWarning("ReCaptcha validation failed.");
                    return Page();
                }
                var user = await signInManager.UserManager.FindByEmailAsync(LModel.Email);

                if (user != null)
                {
   

                    if (user.IsLoggedOn)
                    {
                        LockoutMessage = "User is already logged in.";
                        return Page();
                    }

                    var result = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password,
                        LModel.RememberMe, lockoutOnFailure: true);

                    if (result.Succeeded)
                    {
                        HttpContext.Session.SetString("LoggedIn", LModel.Email);

                        string guid = Guid.NewGuid().ToString();

                        HttpContext.Session.SetString("AuthToken", guid);

                        Response.Cookies.Append("AuthToken", guid, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true, 
                            SameSite = SameSiteMode.None 
                        });

                        TimeZoneInfo SST = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
                        user.LastLogin = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, SST);

						user.IsLoggedOn = true;
                        await signInManager.UserManager.UpdateAsync(user);

                        return RedirectToPage("Index");
                    }

                    if (result.IsLockedOut)
                    {
                        DateTimeOffset? lockoutEnd = await signInManager.UserManager.GetLockoutEndDateAsync(user);

                        if (lockoutEnd.HasValue && lockoutEnd > DateTimeOffset.UtcNow)
                        {
                            TimeSpan remainingTime = lockoutEnd.Value - DateTimeOffset.UtcNow;

                            LockoutMessage = $"Account is locked out. Please try again after {remainingTime.TotalSeconds:F0} seconds.";
                        }
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
        public static bool ReCaptchaPassed(string gRecaptchaResponse, ILogger<LoginModel> logger)
        {

            using (var httpClient = new HttpClient())
            {

                var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret=6Le5qGQpAAAAABsVPihyfJKtIdMTp_0HlvvzUwB3&response={gRecaptchaResponse}").Result;

                if (res.StatusCode != HttpStatusCode.OK)
                {
                    logger.LogError("ReCaptcha verification request failed.");
                    return false;
                }

                string JSONres = res.Content.ReadAsStringAsync().Result;
                dynamic JSONdata = JObject.Parse(JSONres);

                logger.LogInformation($"ReCaptcha Response: {JSONres}");
                logger.LogInformation($"ReCaptcha TokenSDSD: {gRecaptchaResponse}");
                logger.LogInformation($"ReCaptcha Verification Result: {JSONdata.success}");

                // Check if ReCaptcha was successful
                return JSONdata.success;
            }
        }

    }
}


    