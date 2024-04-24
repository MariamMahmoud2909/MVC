﻿using System.ComponentModel.DataAnnotations;

namespace MVC_3PL.ViewModels.Account
{
	public class ForgetPasswordViewModel
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }
	}
}