using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MVC_3DAL.Models
{
	public enum Gender
	{
		[EnumMember(Value = "Male")]
		Male = 1,
		[EnumMember(Value = "Female")]

		Female = 2
	}
	public enum EmpType
	{
		FullTime = 1,
		PartTime = 2
	}
	public class Employee :ModelBase
	{
		//[Required(ErrorMessage = "Name is required")]
		//[MaxLength(50, ErrorMessage = "Max length of name is 50 chars")]
		//[MinLength(5, ErrorMessage = "Min length of name is 5 chars")]
		public string Name { get; set; }
		//[Range(22, 30)]
		public int? Age { get; set; }
		//[RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$", ErrorMessage = "Address must be like 123 Street-City-Country")]
		public string Address { get; set; }
		public EmpType EmployeeType { get; set; }
		public Gender Gender { get; set; }
		//[DataType(DataType.Currency)]
		public decimal Salary { get; set; }
		//[Display(Name = "IsActive")]
		public bool isActive;
		//[EmailAddress]
		public string Email { get; set; }
		//[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }
		//[Display(Name = "Hiring Date")]
		public DateTime HiringDate { get; set; }
		public DateTime CreationDate { get; set; } = DateTime.Now;

        public int? DepartmentId { get; set; } // Foreign Key

        // Navigational Property
        public virtual Department Department { get; set; }

        
    }
}
