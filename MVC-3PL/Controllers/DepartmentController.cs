using Microsoft.AspNetCore.Mvc;
using MVC_3DAL.Models;
using MVC_3BLL.Interfaces;
using MVC_3BLL.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MVC_3PL.Controllers
{
    public class DepartmentController
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository depatmentRepository)
        {
            _departmentRepository = depatmentRepository;
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
    }
}
