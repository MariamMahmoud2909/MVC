using MVC_3DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_3BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
	{
		IQueryable<Employee> GetEmployeeByAddress(string address);
		IQueryable<Employee> SearchByName(string name);
	}
}
