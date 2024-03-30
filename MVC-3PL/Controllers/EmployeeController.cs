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

            if (string.IsNullOrEmpty(searchInput))
                employees = _unitOfWork.EmployeeRepository.GetAll();
            else
                employees = _unitOfWork.EmployeeRepository.SearchByName(searchInput);

            var mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmployees);

        }
        
        [HttpGet]
        public IActionResult Create()
	    {
	    	return View();
	    }

		[HttpPost]
		public IActionResult Create(EmployeeViewModel employeeVM)
		{
            //Employee mappedEmployee = (Employee)employee;
            var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
            
            if (ModelState.IsValid)
            {
                _unitOfWork.EmployeeRepository.Add(mappedEmp);
                
                var count= _unitOfWork.Complete();
                
                if (count > 0)
                    TempData["message"] = "Department is created successfully";
                else
                    TempData["message"] = "An error has occurud while creating department";
                return RedirectToAction(nameof(Index));
            }
            return View(mappedEmp);
        }

        public IActionResult Details(int? id, string ViewName = "Details")
		{
			if (id is null)
				return BadRequest();

		   var employee = _unitOfWork.EmployeeRepository.Get(id.Value);
           var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee);
           
            if (employee is null)
				return NotFound();

			return View(ViewName, mappedEmp);
		}

		public IActionResult Edit(int? id)
		{
			return Details(id, "Edit");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit([FromRoute] int id, EmployeeViewModel employeeVM)
		{

			if (id != employeeVM.Id)
				return BadRequest();

			if (!ModelState.IsValid)
				return View(employeeVM);

			try
			{
				var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
				_unitOfWork.EmployeeRepository.Update(mappedEmp);

				_unitOfWork.Complete();
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

				return View(employeeVM);
			}
		}
		public IActionResult Delete(int? id)
		{
			return Details(id, "Delete");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(EmployeeViewModel employeeVM)
		{
			try
			{
				var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
				_unitOfWork.EmployeeRepository.Delete(mappedEmp);

				_unitOfWork.Complete();
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
