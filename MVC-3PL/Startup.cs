using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC_3DAL.Data;
using MVC_3BLL.Repositories;
using MVC_3BLL.Interfaces;
using MVC_3DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace MVC_3PL
{
    public class Startup
    {
        public IConfiguration Configuration { get; } = null;
        public Startup(IConfiguration configuration) => Configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            //services.AddControllersWithViews();
            services.AddMvc();
            // services.AddTransient<ApplicationDbContext>();
            // services.AddScoped<ApplicationDbContext>();
            // services.AddSingleton<ApplicationDbContext>();
            //services.AddScoped<DbContextOptions<ApplicationDbContext>>();

            services.AddDbContext<ApplicationDbContext>
                (
                options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                }
                );

            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
			services.AddScoped<IEmployeeRepository, EmployeeRepository>();

		}
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
