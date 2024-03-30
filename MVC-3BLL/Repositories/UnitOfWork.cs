using MVC_3BLL.Interfaces;
using MVC_3DAL.Data;
using MVC_3DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_3BLL.Repositories
{
	public class UnitOfWork : IUnitOfWork, IDisposable
	{
		private readonly ApplicationDbContext _dbContext;

		//private Dictionary<string, IGenericRepository<ModelBase>> _repositories;

		private Hashtable _repositories;

		public UnitOfWork(ApplicationDbContext dbContext) // Ask CLR to create object from dbcontext class
		{
			_dbContext = dbContext;
			_repositories = new Hashtable();
		}

		public int Complete()
		{
			return _dbContext.SaveChanges();
		}
		public void Dispose()
		{
			_dbContext.Dispose(); //closes the database conection
		}

		public IGenericRepository<T> Repository<T>() where T : ModelBase
		{
			var key = typeof(T).Name; //Employee

			if (!_repositories.ContainsKey(key))
			{
				if (key == nameof(Employee))
				{
					var repository = new EmployeeRepository(_dbContext);
					_repositories.Add(key, repository);
				}
				else
				{
					var repository = new GenericRepository<T>(_dbContext);
					_repositories.Add(key, repository);
				}

			}

			return _repositories[key] as IGenericRepository<T>;
		}
	}
}
