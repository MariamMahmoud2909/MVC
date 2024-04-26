using System.ComponentModel.DataAnnotations;

namespace MVC_3PL.ViewModels.User
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Password is required")]
        //[MinLength(5 , ErrorMessage = "Minumum password length is 5")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm password doesn't match with new password")]
        public string ConfirmPassword { get; set; }
    }
}