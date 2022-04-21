using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentJournal.AppDatabase;

/// <summary>
/// DbContext for authorization and authentication
/// </summary>
public class AppIdentityDbContext : IdentityDbContext<User>
{
    public AppIdentityDbContext(
        DbContextOptions<AppIdentityDbContext> options,
        IServiceProvider serviceProvider, 
        IConfiguration configuration) : base(options) 
    {
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
        string role = _configuration["AdminUser:Role"];

        if (await userManager.FindByNameAsync(username) == null)
        {
            if (await roleManager.FindByNameAsync(role) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            User user = new User
            {
                UserName = username,
                Email = email
            };

            IdentityResult result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }

    public async Task CreateRoles() 
    {
        var roles = new List<string>()
        {
            Constants.ConsumersRole,
            Constants.ProducersRole
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