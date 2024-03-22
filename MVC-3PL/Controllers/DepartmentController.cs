using Microsoft.AspNetCore.Mvc;
using MVC_3DAL.Models;
using MVC_3BLL.Interfaces;
using MVC_3BLL.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MVC_3PL.Controllers
{
    public class DepartmentController : Controller
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


        // /Department/Details/10 
        // /Department/Details 

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (!id.HasValue)
                return BadRequest(); // 400
            var department = _departmentRepository.Get(id.Value);

            if (department is null)
                return NotFound(); // 404 

            return View(department);
        }
    }
}
