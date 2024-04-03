using System.ComponentModel.DataAnnotations;

namespace MVC_3PL.ViewModels
{
	public class SignupViewModel
	{
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		[DataType(DataType.Password)]
		[Required]
		public string Passoword { get; set; }
		[DataType(DataType.Password)]
		[Required]
		[Compare(nameof(Passoword), ErrorMessage = "Password Not Matched")]
		public string ConfirmPassword { get; set; }
		public bool IsAgree { get; set; }
	}
}
