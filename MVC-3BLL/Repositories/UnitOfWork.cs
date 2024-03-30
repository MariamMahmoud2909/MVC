using MVC_3BLL.Interfaces;
using MVC_3DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_3BLL.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _dbContext;

		public UnitOfWork(ApplicationDbContext dbContext) // Ask CLR to create object from dbcontext class
		{
			_dbContext = dbContext;
			EmployeeRepository = new EmployeeRepository(_dbContext);
			DepartmentRepository = new DepartmentRepository(_dbContext);
			
		}

		public IEmployeeRepository EmployeeRepository { get; set; }
		public IDepartmentRepository DepartmentRepository { get; set; }
		public int Complete()
		{
			return _dbContext.SaveChanges();
		}
	}
}
