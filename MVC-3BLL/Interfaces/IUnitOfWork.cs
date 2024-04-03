using MVC_3DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_3BLL.Interfaces
{
	public interface IUnitOfWork : IAsyncDisposable
	{
		IGenericRepository<T> Repository<T>() where T: ModelBase;

		public Task<int> Complete();
	}
}
