using Microsoft.AspNetCore.Mvc;
using MVC_3DAL.Models;
using MVC_3BLL.Interfaces;
using MVC_3BLL.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using System.Collections.Generic;
using MVC_3PL.ViewModels;

namespace MVC_3PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper mapper;

        public DepartmentController(IUnitOfWork unitOfWork, IWebHostEnvironment env, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _env = env;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            var mappedDepartments = mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(mappedDepartments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DepartmentViewModel departmentVM)
        {
            var mappedDep = mapper.Map<DepartmentViewModel, Department>(departmentVM);
            if (ModelState.IsValid) // server side validation
            {
                _unitOfWork.DepartmentRepository.Add(mappedDep);
                var count = _unitOfWork.Complete();
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(mappedDep);
        }

        // /Department/Details/10 
        // /Department/Details 

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)//id is null
                return BadRequest();

            var department = _unitOfWork.DepartmentRepository.Get(id.Value);
            _unitOfWork.Complete();
            if (department is null)
                return NotFound();
            var mappedDep = mapper.Map<Department, DepartmentViewModel>(department);
            return View(viewName, mappedDep);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
            {
                return BadRequest(new ViewResult());
            }
            if (!ModelState.IsValid)
            {
                return View(departmentVM);
            }
            try
            {
                var mappedDep = mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfWork.DepartmentRepository.Update(mappedDep);
				_unitOfWork.Complete();
				return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                //log exception
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, ("An error has occured during updating the department"));
                return View(departmentVM);
            }
        }
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        public IActionResult Delete(DepartmentViewModel departmentVM )
        {
            try
            {
                var mappedDep = mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfWork.DepartmentRepository.Delete(mappedDep);
				_unitOfWork.Complete();
				return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, ("An error has occured during updating the department"));
                return View(departmentVM);
                //return View("Error", new ErrorViewModel());
            }
        }

        ///Delete using modal
        ///[HttpPost]
        ///[ValidateAntiForgeryToken]
        ///public IActionResult Delete(int id)
        ///{
        ///	var departmentToDelete = _departmentRepo.Get(id);
        ///	if (departmentToDelete == null)
        ///	{
        ///		return NotFound(); 
        ///	}
        ///	_departmentRepo.Delete(departmentToDelete); 
        ///	return RedirectToAction("Index");
        ///}

    }
}
