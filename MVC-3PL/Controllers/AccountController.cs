using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using MVC_3DAL.Models;
using MVC_3PL.Services.EmailSender;
using MVC_3PL.ViewModels.Account;
using System.Threading.Tasks;

namespace MVC_3PL.Controllers
{
    public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		private readonly IEmailSender _emailSender;
		private readonly IConfiguration _configuration;

		public AccountController(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IEmailSender emailSender,
			IConfiguration configuration)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_emailSender = emailSender;
			_configuration = configuration;
		}

		#region SignUp - Register


		public IActionResult SignUp()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SignUp(SignupViewModel model)
		{

			if (ModelState.IsValid)
			{
				var userEmail = await _userManager.FindByEmailAsync(model.Email);
				if (userEmail is null)
				{
					var user = new ApplicationUser
					{
						UserName = model.UserName,
						Email = model.Email,
						FirstName = model.FirstName,
						LastName = model.LastName,
						IsAgree = model.IsAgree,
					};

					var result = await _userManager.CreateAsync(user, model.Passoword);
					if (result.Succeeded)
					{
						return RedirectToAction(nameof(SignIn));
					}
					else
					{
						foreach (var error in result.Errors)
							ModelState.AddModelError(string.Empty, error.Description);
					}
				};
				ModelState.AddModelError(string.Empty, "This user Is Already Exist");
			}
			return View(model);
		}

		public IActionResult SignIn()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var flag = await _userManager.CheckPasswordAsync(user, model.Password);
					if (flag)
					{
						var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
						if (result.IsLockedOut)
						{
							ModelState.AddModelError(string.Empty, "Your account is locked");
						}
						if (result.Succeeded)
						{
							return RedirectToAction(nameof(HomeController.Index), "Home");
						}
						if (result.IsNotAllowed)
						{
							ModelState.AddModelError(string.Empty, "Your account is not confirmed yet"); ;
						}

					}
				}
				ModelState.AddModelError(string.Empty, "Invalid login");
			}
			return View(model);

		}

		public async new Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(SignIn));
		}

		public IActionResult ForgetPassword()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SendResetPasswordEmail(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);

				if (user is not null)
				{
					var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = user.Email, token = resetPasswordToken }, "https", "localhost:5001");
					await _emailSender.SendAsync(
						from: _configuration["EmailSettings:SenderEmail"],
						recipients: model.Email,
						subject: "Reset Your Password",
						body: resetPasswordUrl
						);
					return RedirectToAction(nameof(CheckYourInbox));

					ModelState.AddModelError(string.Empty, "There is not account with this email");
				}
				return View(model);
			}
		}

			public IActionResult CheckYourInbox()
			=> View();
			#endregion

			#region Reset Password

			public IActionResult ResetPassword(string email, string token)
			{
				TempData["Email"] = email;
				TempData["token"] = token;

				return View();
			}

			[HttpPost]
			public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
			{

				if (ModelState.IsValid)
				{
					var email = TempData["Email"] as string;
					var token = TempData["token"] as string;

					var user = await _userManager.FindByEmailAsync(email);

					if (user is not null)
					{
						await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
						return RedirectToAction(nameof(SignIn));
					}

					ModelState.AddModelError(string.Empty, "Url is not valid");
				}

				return View(model);
			}

			#endregion
		}
	}
