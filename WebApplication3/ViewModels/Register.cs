using System.ComponentModel.DataAnnotations;

namespace Assignment_2.ViewModels
{
	public class Register
	{

		[Required]
		[DataType(DataType.Text)]
		public string? FullName { get; set; }

		[Required]
		[DataType(DataType.CreditCard)]
		public string? CreditCardNo { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string? Gender { get; set; }

		[Required]
		[DataType(DataType.PhoneNumber)]
		public string? MobileNo { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string? DeliveryAddress { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		public string? Email { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		[DataType(DataType.Password)]
		[MinLength(12, ErrorMessage = "Password must be at least 12 characters long.")]
		[RegularExpression(@"(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{12,}", ErrorMessage = "Password should include lower-case, upper-case, numbers, and special characters.")]
		public string? Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match")]

		public string? ConfirmPassword { get; set; }

		[DataType(DataType.Upload)]
		[FileExtensions(Extensions ="jpg,jpeg", ErrorMessage = "Invalid file format. Only .JPG/.JPEG files are allowed.")]
		public IFormFile? Photo { get; set; }

		[DataType(DataType.MultilineText)]
		[Display(Name = "About Me")]
		public string? AboutMe { get; set; }

		[DataType(DataType.DateTime)]
        public DateTime? LastLogin { get; set; }

    }
}
