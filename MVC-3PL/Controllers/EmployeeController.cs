using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC_3BLL.Interfaces;
using MVC_3BLL.Repositories;
using MVC_3DAL.Models;
using System.Linq;
using System;

namespace MVC_3PL.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly IEmployeeRepository _employeeRepository;
			private readonly IWebHostEnvironment _env;

			public EmployeeController(IEmployeeRepository employeeRepository, IWebHostEnvironment env)
			{
				_employeeRepository = employeeRepository;
				_env = env;
			}
			public IActionResult Index(string searchInput)
			{
				var employees = Enumerable.Empty<Employee>();

				if (string.IsNullOrEmpty(searchInput))
					employees = _employeeRepository.GetAll();
				else
					employees = _employeeRepository.SearchByName(searchInput);

				return View(employees);
			}
			public IActionResult Create()
			{
				return View();
			}
			[HttpPost]
			public IActionResult Create(Employee employee)
			{
				if (ModelState.IsValid) // server side validation
				{
					var count = _employeeRepository.Add(employee);

					if (count > 0)
						TempData["Message"] = "Employee is Created Successfully";
					else
						TempData["Message"] = "An Error Has Occured, Employee Not Created :(";

					return RedirectToAction(nameof(Index));
				}
				return View(employee);
			}

			public IActionResult Details(int? id, string ViewName = "Details")
			{
				if (id is null)
					return BadRequest();
				var employee = _employeeRepository.Get(id.Value);

				if (employee is null)
					return NotFound();

				return View(ViewName, employee);
			}


			public IActionResult Edit(int? id)
			{
				return Details(id, "Edit");
			}

			[HttpPost]
			[ValidateAntiForgeryToken]
			public IActionResult Edit([FromRoute] int id, Employee employee)
			{

				if (id != employee.Id)
					return BadRequest();

				if (!ModelState.IsValid)
					return View(employee);

				try
				{
					_employeeRepository.Update(employee);
					return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					// 1. log Exception
					// 2. Friendly Message
					if (_env.IsDevelopment())
						ModelState.AddModelError(string.Empty, ex.Message);
					else
						ModelState.AddModelError(string.Empty, "An Error Has Occurred during Updating the employee");

					return View(employee);
				}
			}

			[HttpPost]
			public IActionResult Delete(int? id)
			{
				if (!id.HasValue)
					return BadRequest();

				var employee = _employeeRepository.Get(id.Value);

				if (employee is null)
					return NotFound();

				try
				{
					_employeeRepository.Delete(employee);
				}
				catch (Exception ex)
				{
					if (_env.IsDevelopment())
						ModelState.AddModelError(string.Empty, ex.Message);
					else
						ModelState.AddModelError(string.Empty, "An Error Has Occurred during Deleting the employee");

					return RedirectToAction(nameof(Index));
				}

				return RedirectToAction(nameof(Index));
			}

		}
	}
}
