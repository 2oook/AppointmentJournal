using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AppointmentJournal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using AppointmentJournal.AppReversedDatabase;

namespace AppointmentJournal
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

            services.AddDbContext<AppointmentJournalContext>(options => options.UseSqlServer(Configuration["Data:AppointmentJournal:ConnectionString"]));

            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration["Data:AppointmentJournalIdentity:ConnectionString"]));

            services.AddTransient<IServiceRepository, FakeServiceRepository>();
            //services.AddTransient<IServiceRepository, EFServiceRepository>();

            services.AddControllersWithViews();

            // включить компил€цию razor - страниц во врем€ выполнени€
            services.AddRazorPages().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // —оздать аккаунт администратора, если его нет
            AppIdentityDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();

            // создать роли
            //AppIdentityDbContext.CreateRoles(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                // по - возможности переместить вызов
                FakeServiceRepository.CreateFakeUsers(app.ApplicationServices);
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "empty",
                    pattern: "{controller=Consumer}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller}/{action}/{id?}");


            });
        }
    }
}
