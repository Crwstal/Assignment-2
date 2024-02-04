using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

public class ApplicationUser : IdentityUser
{
	public string? FullName { get; set; }
	public string? CreditCardNo { get; set; }
	public string? Gender { get; set; }
	public string? MobileNo { get; set; }
	public string? DeliveryAddress { get; set; }

	public string? PhotoFile { get; set; }
	public string? AboutMe { get; set; }
    public DateTime? LastLogin { get; set; }

    public bool IsLoggedOn { get; set; }

}
