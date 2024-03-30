using MVC_3DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_3BLL.Interfaces
{
	public interface IUnitOfWork
	{
		IEmployeeRepository EmployeeRepository { get; set; }
		IDepartmentRepository DepartmentRepository { get; set; }

		//IGenericRepository<Department> DepartmentRepository { get; set; }

		int Complete();
	}
}
