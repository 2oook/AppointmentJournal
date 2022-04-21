using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AppointmentJournal.AppCore;

namespace AppointmentJournal.AppDatabase;

/// <summary>
/// DbContext for authorization and authentication
/// </summary>
public class AppIdentityDbContext : IdentityDbContext<User>
{
    public AppIdentityDbContext(
        DbContextOptions<AppIdentityDbContext> options,
        IServiceProvider serviceProvider, 
        IConfiguration configuration,
        IConfig config) : base(options) 
    {
        Database.SetConnectionString(config.SqlSettings.IdentityConnectionString);
        Database.EnsureCreated();
        
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    private IServiceProvider _serviceProvider { get; set; }

    private IConfiguration _configuration { get; set; }

    public async Task CreateAdminAccount()
    {
        UserManager<User> userManager = _serviceProvider.GetRequiredService<UserManager<User>>();
        RoleManager<IdentityRole> roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string username = _configuration["AdminUser:Name"];
        string email = _configuration["AdminUser:Email"];
        string password = _configuration["AdminUser:Password"];

        if (await userManager.FindByNameAsync(username) == null)
        {
            User user = new User
            {
                UserName = username,
                Email = email
            };

            IdentityResult result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, DatabaseConstants.AdminsRole);
            }
        }
    }

    public async Task CreateRoles() 
    {
        var roles = new List<string>()
        {
            DatabaseConstants.ConsumersRole,
            DatabaseConstants.ProducersRole,
            DatabaseConstants.AdminsRole
        };

        RoleManager<IdentityRole> roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (var role in roles)
        {
            if (await roleManager.FindByNameAsync(role) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }        
    }
}