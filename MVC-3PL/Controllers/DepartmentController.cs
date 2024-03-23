using Microsoft.AspNetCore.Mvc;
using MVC_3DAL.Models;
using MVC_3BLL.Interfaces;
using MVC_3BLL.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MVC_3PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(IDepartmentRepository depatmentRepository, IWebHostEnvironment env)
        {
            _departmentRepository = depatmentRepository;
            _env = env;
        }
        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid) // server side validation
            {
                var count = _departmentRepository.Add(department);
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // /Department/Details/10 
        // /Department/Details 

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)//id is null
                return BadRequest();

            var department = _departmentRepository.Get(id.Value);

            if (department is null)
                return NotFound();
            return View(viewName, department);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Department department)
        {
            if (id != department.Id)
            {
                return BadRequest(new ViewResult());
            }
            if (!ModelState.IsValid)
            {
                return View(department);
            }
            try
            {
                _departmentRepository.Update(department);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                //log exception
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, ("An error has occured during updating the department"));
                return View(department);
            }
        }
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        public IActionResult Delete(Department department)
        {
            try
            {
                _departmentRepository.Delete(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, ("An error has occured during updating the department"));
                return View(department);
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
