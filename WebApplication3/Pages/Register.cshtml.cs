using Assignment_2.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication3.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public Register RModel { get; set; }

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IDataProtectionProvider dataProtectionProvider;

        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IDataProtectionProvider dataProtectionProvider)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dataProtectionProvider = dataProtectionProvider;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var existingUser = await userManager.FindByEmailAsync(RModel.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("RModel.Email", "Email address is already in use.");
                return Page();
            }

            if (ModelState.IsValid)
            {
                var Encrypt = DataProtectionProvider.Create("EncryptData");
                var protector = Encrypt.CreateProtector("MySecretKey");

                var user = new ApplicationUser()
                {
                    UserName = RModel.Email,
                    Email = RModel.Email,
                    FullName = RModel.FullName,
                    CreditCardNo = protector.Protect(RModel.CreditCardNo),
                    Gender = RModel.Gender,
                    MobileNo = RModel.MobileNo,
                    DeliveryAddress = RModel.DeliveryAddress,
                    Photo = RModel.Photo,
                    AboutMe = RModel.AboutMe,
                    LastLogin = RModel.LastLogin,
                    IsLoggedOn = false
                };

                var result = await userManager.CreateAsync(user, RModel.Password);

                if (result.Succeeded)
                {
                    //await signInManager.SignInAsync(user, false);
                    return RedirectToPage("Login");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return Page();
        }
    }
}
