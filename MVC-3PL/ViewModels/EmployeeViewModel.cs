using MVC_3DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace MVC_3PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Max length of name is 50 chars")]
        [MinLength(5, ErrorMessage = "Min length of name is 5 chars")]
        public string Name { get; set; }
        [Range(22, 30)]
        public int? Age { get; set; }
        //[RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",ErrorMessage ="Address must be like 123 Street-City-Country")]
        public string Address { get; set; }
        public EmpType EmployeeType { get; set; }
        public Gender Gender { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Display(Name = "IsActive")]
        public bool isActive;
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Hiring Date")]
        public DateTime HiringDate { get; set; }
        //ForeginKey
        public int? DepartmentId { get; set; }

        //Navigational property
        //[InverseProperty(nameof(Models.Department.Employees))]
        public Department Department { get; set; }
    }
}
