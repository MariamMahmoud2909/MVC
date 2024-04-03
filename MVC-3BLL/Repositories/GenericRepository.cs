using Microsoft.EntityFrameworkCore;
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
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
	{
		private protected readonly ApplicationDbContext _dbContext;
		public GenericRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public void Add(T entity)
	       => _dbContext.Set<T>().Add(entity);

		public void Delete(T entity)
		{
			_dbContext.Set<T>().Remove(entity);
		}

		public async Task<T> GetAsync(int id)
		{
			return await _dbContext.FindAsync<T>(id);
		}

		public IEnumerable<T> GetAllAsync()
		{
			if (typeof(T) == typeof(Employee))
			{
				return (IEnumerable<T>)_dbContext.Employees.Include(E => E.Department).AsNoTracking().ToList();
			}
			else
			{
				return _dbContext.Set<T>().AsNoTracking().ToList();
			}
		}

		public void Update(T entity)
		{
			_dbContext.Set<T>().Update(entity);
		}
	}
}
