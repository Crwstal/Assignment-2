using System.ComponentModel.DataAnnotations;

namespace Assignment_2.ViewModels
{
	public class Register
	{

		[Required(ErrorMessage="Full Name required.")]
		[DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Full Name only allows alphabetical characters.")]
        public string? FullName { get; set; }

		[Required(ErrorMessage = "Credit Card required.")]
		[DataType(DataType.CreditCard)]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Invalid credit card number. It should be a numeric 16-digit code.")]
        public string? CreditCardNo { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string? Gender { get; set; }

		[Required(ErrorMessage = "Phone Number required.")]
		[DataType(DataType.PhoneNumber)]
		[RegularExpression(@"^(\+\d{1,2})?\d{8,15}$", ErrorMessage = "Invalid phone number format.")]
		public string? MobileNo { get; set; }

		[Required(ErrorMessage = "Delivery Address required.")]
		[DataType(DataType.Text)]
		[RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Delivery Address only allows alphanumeric characters.")]
		public string? DeliveryAddress { get; set; }

		[Required(ErrorMessage = "Email is required.")]
		[DataType(DataType.EmailAddress)]
		[EmailAddress(ErrorMessage = "Invalid email address.")]
		public string? Email { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		[DataType(DataType.Password)]
		[MinLength(12, ErrorMessage = "Password must be at least 12 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{12,}$",
			 ErrorMessage = "Password should include lower-case, upper-case, numbers, and special characters.")]
        public string? Password { get; set; }

		[Required(ErrorMessage = "Confirm Password required.")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match.")]

		public string? ConfirmPassword { get; set; }

		[Required]
		[DataType(DataType.Upload)]
		public IFormFile? Photo { get; set; }

		[DataType(DataType.MultilineText)]
        [MaxLength(500, ErrorMessage = "About Me should not exceed 500 characters.")]
        [Required(ErrorMessage = "About Me required.")]
		public string? AboutMe { get; set; }

		[DataType(DataType.DateTime)]
        public DateTime? LastLogin { get; set; }

    }
}
