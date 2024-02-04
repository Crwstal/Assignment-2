using System.ComponentModel.DataAnnotations;

namespace Assignment_2.ViewModels
{
    public class Login
    {
		[Required(ErrorMessage = "Email is required.")]
		[DataType(DataType.EmailAddress)]
		[EmailAddress(ErrorMessage = "Invalid email address.")]
		public string? Email { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		[DataType(DataType.Password)]
		[MinLength(12, ErrorMessage = "Password must be at least 12 characters long.")]
		[RegularExpression(@"(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{12,}", 
			ErrorMessage = "Password should include lower-case, upper-case, numbers, and special characters.")]
		public string? Password { get; set; }
		public bool RememberMe { get; set; }
    }
}
