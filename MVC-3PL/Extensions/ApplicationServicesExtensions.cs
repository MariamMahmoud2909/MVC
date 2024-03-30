using Microsoft.Extensions.DependencyInjection;
using MVC_3BLL.Interfaces;
using MVC_3BLL.Repositories;
using MVC_3PL.Helpers;

namespace MVC_3PL.Extensions
{
	public static class ApplicationServicesExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped<IUnitOfWork, UnitOfWork>();	
			//services.AddScoped<IDepartmentRepository, DepartmentRepository>();
			//services.AddScoped<IEmployeeRepository, EmployeeRepository>();
			//services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));

			return services;
		}
	}
}
