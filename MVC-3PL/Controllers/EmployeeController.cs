using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC_3BLL.Interfaces;
using MVC_3BLL.Repositories;
using MVC_3DAL.Models;
using System.Linq;
using System;
using AutoMapper;
using System.Collections;
using System.Collections.Generic;
using MVC_3PL.ViewModels;
using System.Reflection.Metadata;
using MVC_3PL.Helpers;
using System.Threading.Tasks;

namespace MVC_3PL.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHostEnvironment _env;
        private readonly IMapper _mapper;
        public EmployeeController(IUnitOfWork unitOfWork,IHostEnvironment env, IMapper mapper)
        {
			_unitOfWork = unitOfWork;
			_env = env;
            _mapper = mapper;
        }
        public IActionResult Index(string searchInput)
        {
            var employees = Enumerable.Empty<Employee>();
			var employeeRepo = _unitOfWork.Repository<Employee>() as EmployeeRepository;
            
			if (string.IsNullOrEmpty(searchInput))
                employees = employeeRepo.GetAllAsync();
            else
                employees = employeeRepo.SearchByName(searchInput);

            var mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmployees);

        }
        
        [HttpGet]
        public IActionResult Create()
	    {
			//ViewBag.UnitOfWork = _unitOfWork;
	    	return View();
	    }

		[HttpPost]
		public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
		{
			employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "images");
			//Employee mappedEmployee = (Employee)employee;
			var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

            if (ModelState.IsValid)
            {
                _unitOfWork.Repository<Employee>().Add(mappedEmp);
                
                var count= await _unitOfWork.Complete();
                
                if (count > 0)
				{
					TempData["message"] = "Department is created successfully";
					return RedirectToAction(nameof(Index));
				}
				else
                    TempData["message"] = "An error has occurud while creating department";
            }
            return View(mappedEmp);
        }

        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
		{
			if (id is null)
				return BadRequest();

		   var employee = await _unitOfWork.Repository<Employee>().GetAsync(id.Value);
           var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee);
           
            if (employee is null)
				return NotFound();

			if (ViewName.Equals(nameof(Delete), StringComparison.OrdinalIgnoreCase))
			{
				TempData["ImageName"] = employee.ImageName;
			}

			return View(ViewName, mappedEmp);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			return await Details(id, "Edit");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
		{

			if (id != employeeVM.Id)
				return BadRequest();

			if (!ModelState.IsValid)
				return View(employeeVM);

			try
			{
				employeeVM.ImageName = TempData["ImageName"] as string;

				var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
				_unitOfWork.Repository<Employee>().Update(mappedEmp);

				var count = await _unitOfWork.Complete();
				if (count > 0)
				{
					DocumentSettings.DeleteFile(employeeVM.ImageName, "images");
					return RedirectToAction(nameof(Index));
				}
				return View(employeeVM);
			}
			catch (Exception ex)
			{
				// 1. log Exception
				// 2. Friendly Message
				if (_env.IsDevelopment())
					ModelState.AddModelError(string.Empty, ex.Message);
				else
					ModelState.AddModelError(string.Empty, "An Error Has Occurred during Updating the employee");

				return View(employeeVM);
			}
		}
		public async Task<IActionResult> Delete(int? id)
		{
			return await Details(id, "Delete");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(EmployeeViewModel employeeVM)
		{
			try
			{
				var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
				_unitOfWork.Repository<Employee>().Delete(mappedEmp);

				await _unitOfWork.Complete();
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				if (_env.IsDevelopment())
					ModelState.AddModelError(string.Empty, ex.Message);
				else
					ModelState.AddModelError(string.Empty, ("An error has occured during update the employee"));
				return View(employeeVM);
			}

		}
	}
}
