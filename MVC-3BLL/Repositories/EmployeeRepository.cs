﻿using Microsoft.EntityFrameworkCore;
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
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
	{
		public EmployeeRepository(ApplicationDbContext dbContext) : base(dbContext)
		{

		}

		public IQueryable<Employee> GetEmployeeByAddress(string address)
		{
			return _dbContext.Employees.Where(E => E.Address.Equals(address, StringComparison.OrdinalIgnoreCase));
		}
        
		public override IEnumerable<Employee> GetAll()
        => _dbContext.Employees.Include(E => E.Department).ToList();

		public IQueryable<Employee> SearchByName(string name)
			=> _dbContext.Employees.Where(E => E.Name.ToLower().Contains(name.ToLower())).Include(E => E.Department);

	}
	
}
