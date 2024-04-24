using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVC_3DAL.Models;
using MVC_3PL.ViewModels;
using System.Threading.Tasks;
using TaskThree.PL.ViewModels.Account;

namespace MVC_3PL.Controllers
{
	public class AccountController : Controller
	{
		#region SignUp - Register

		private readonly UserManager<ApplicationUser> _userManager;

		public AccountController(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}
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
				var user = await userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var flag = await userManager.CheckPasswordAsync(user, model.Password);
					if (flag)
					{
						var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
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

		#endregion
	}
}
