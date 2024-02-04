using Assignment_2.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Text;
using System.Web;

namespace WebApplication3.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public Register RModel { get; set; }

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public RegisterModel(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(RModel.Email))
            {
                ModelState.AddModelError(nameof(RModel.Email), "Email is required.");
                return Page();
            }
            var existingUser = await userManager.FindByEmailAsync(RModel.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("RModel.Email", "Email address is already in use.");
                return Page();
            }

            if (ModelState.IsValid)
            {
                if (RModel.Photo != null)
                {
                    string[] allowedExtensions = { ".jpg", ".jpeg" };
                    string fileExtension = Path.GetExtension(RModel.Photo.FileName);

                    // Check if the file extension is not allowed
                    if (!allowedExtensions.Contains(fileExtension.ToLower()))
                    {
                        ModelState.AddModelError("RModel.Photo", "Only .jpg or .jpeg files are allowed.");
                        return Page();
                    }

                    // Rest of the code for file upload
                    string folder = "images/";
                    folder += RModel.Photo.FileName;
                    string serverfolder = Path.Combine(webHostEnvironment.WebRootPath, folder);

                    await RModel.Photo.CopyToAsync(new FileStream(serverfolder, FileMode.Create));
                }
                var Encrypt = DataProtectionProvider.Create("EncryptData");
                var protector = Encrypt.CreateProtector("MySecretKey");

				var user = new ApplicationUser()
				{
					UserName = RModel.Email,
					Email = RModel.Email,
					FullName = HtmlEncode(RModel.FullName),
					CreditCardNo = protector.Protect(RModel.CreditCardNo),
					Gender = HtmlEncode(RModel.Gender),
					MobileNo = HtmlEncode(RModel.MobileNo),
					DeliveryAddress = HtmlEncode(RModel.DeliveryAddress),
					AboutMe = HtmlEncode(RModel.AboutMe),
					PhotoFile = HtmlEncode(RModel.Photo.FileName),
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
		private string HtmlEncode(string data)
		{
			return HttpUtility.HtmlEncode(data);
		}

	}
}
