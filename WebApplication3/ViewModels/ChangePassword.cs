using System.ComponentModel.DataAnnotations;

namespace Assignment_2.ViewModels
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Current Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(12, ErrorMessage = "Password must be at least 12 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{12,}$",
            ErrorMessage = "Password should include lower-case, upper-case, numbers, and special characters.")]
        public string? CurrentPassword { get; set; }

        [Required(ErrorMessage = "New Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(12, ErrorMessage = "Password must be at least 12 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{12,}$",
            ErrorMessage = "Password should include lower-case, upper-case, numbers, and special characters.")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "New Password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
