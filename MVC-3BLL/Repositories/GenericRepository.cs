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
		{
			_dbContext.Add(entity);
		}

		public void Delete(T entity)
		{
			_dbContext.Remove(entity);
		}

		public T Get(int id)
		{
			return _dbContext.Find<T>(id);
		}

		virtual public IEnumerable<T> GetAll()
			=> _dbContext.Set<T>().ToList();

		public void Update(T entity)
		{
			_dbContext.Update(entity);
		}
	}
}
