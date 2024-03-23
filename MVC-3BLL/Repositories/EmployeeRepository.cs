using MVC_3BLL.Interfaces;
using MVC_3DAL.Data;
using MVC_3DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_3BLL.Repositories
{
	internal class EmployeeRepository : IEmployeeRepository
	{
		private readonly ApplicationDbContext _dbContext;
		public EmployeeRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public int Add(Employee entity)
		{
			_dbContext.Add(entity);
			return _dbContext.SaveChanges();
		}

		public int Delete(Employee entity)
		{
			_dbContext.Remove(entity);
			return _dbContext.SaveChanges();
		}

		public Employee Get(int id)
		{
			///var Employee = _dbContext.Employees.Local.Where(D => D.Id == id).FirstOrDefault();
			///if(Employee == null)
			///    Employee = _dbContext.Employees.Where(D => D.Id == id).FirstOrDefault();
			///return Employee;
			///
			return _dbContext.Employees.Find(id);
		}

		public IEnumerable<Employee> GetAll()
			=> _dbContext.Employees.ToList();

		public int Update(Employee entity)
		{
			_dbContext.Update(entity);
			return _dbContext.SaveChanges();
		}
	}
}
