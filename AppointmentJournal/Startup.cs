using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AppointmentJournal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using AppointmentJournal.AppReversedDatabase;

namespace AppointmentJournal;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

        services.AddDbContext<AppointmentJournalContext>(options => 
            options.UseSqlServer(Configuration["Data:AppointmentJournal:ConnectionString"]));

        services.AddDbContext<AppIdentityDbContext>(options => 
            options.UseSqlServer(Configuration["Data:AppointmentJournalIdentity:ConnectionString"]));

        services.AddControllersWithViews();

        services
            .AddRazorPages()
            .AddRazorRuntimeCompilation();
    }

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

        AppIdentityDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();
        AppIdentityDbContext.CreateRoles(app.ApplicationServices).Wait();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: null,
                pattern: "{category}/Page{servicePage:int}",
                defaults: new { controller = "Home", action = "List", servicePage = 1 });

            endpoints.MapControllerRoute(
                name: "empty",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}");
        });
    }
}