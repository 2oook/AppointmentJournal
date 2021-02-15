using AppointmentJournal.AppReversedDatabase;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.Models
{
    public class FakeServiceRepository : IServiceRepository
    {
        private ServicesCategory autoCategory = new ServicesCategory() { Name = "Автомобиль" };

        public IQueryable<Service> Services => new List<Service>() 
        {
            new Service() 
            {
                Appointments = new List<Appointment>()
                { 
                    new Appointment()
                    { 
                        Address = new Address(){ AddressValue = "ул.Ленина, д.100" },
                        Time = new DateTime(2021, 4, 15, 12, 00, 00),
                        WorkDay = new WorkDay()
                        { 
                            WorkDaysTimeSpans = new List<WorkDaysTimeSpan>()
                            { 
                                new WorkDaysTimeSpan()
                                {
                                    BeginTime = new DateTime(2021, 4, 15, 10, 00, 00),
                                    EndTime = new DateTime(2021, 4, 15, 19, 00, 00)
                                }                              
                            } 
                        }
                    }
                },
                Duration = 30,
                Name = "Мойка снаружи",
                Price = 200,
                Category = autoCategory,
                


            }
        }.AsQueryable();

        // Метод для создания тестовых пользователей
        public static async Task CreateFakeUsers(IServiceProvider serviceProvider) 
        {
            UserManager<User> userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var users = new List<(string name, string email, string password, string role)>()
            {
                ("user1", "user1@email.com", "@Qaz123usr", "Customers"),
                ("user2", "user2@email.com", "@Qaz123usr", "Customers"),
                ("user3", "user3@email.com", "@Qaz123usr", "Customers"),

                ("producer1", "producer1@email.com", "@Qaz123usr", "Producers"),
                ("producer2", "producer2@email.com", "@Qaz123usr", "Producers"),
                ("producer3", "producer3@email.com", "@Qaz123usr", "Producers")
            };

            foreach (var user in users)
            {
                if (await userManager.FindByNameAsync(user.name) == null)
                {
                    if (await roleManager.FindByNameAsync(user.role) == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole(user.role));
                    }

                    User new_user = new User
                    {
                        UserName = user.name,
                        Email = user.email
                    };

                    IdentityResult result = await userManager.CreateAsync(new_user, user.password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(new_user, user.role);
                    }
                }
            }         
        }
    }
}
