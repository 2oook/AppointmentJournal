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
        public FakeServiceRepository()
        {
            if (usersDictionary == null || usersDictionary.Count == 0)
            {
                CreateFakeUsers();
            }

            workDays = new Dictionary<int, WorkDay>()
            {
                {
                    0,
                    new WorkDay()
                    {
                        Id = 0,
                        ProducerId = usersDictionary["car_washer"].Id,
                        WorkDaysTimeSpans = new List<WorkDaysTimeSpan>()
                        {
                            new WorkDaysTimeSpan()
                            {
                                Id = 0,
                                BeginTime = new DateTime(2021, 4, 15, 10, 00, 00),
                                EndTime = new DateTime(2021, 4, 15, 19, 00, 00)
                            }
                        }
                    }
                },
                {
                    1,
                    new WorkDay()
                    {
                        Id = 1,
                        ProducerId = usersDictionary["barber"].Id,
                        WorkDaysTimeSpans = new List<WorkDaysTimeSpan>()
                        {
                            new WorkDaysTimeSpan()
                            {
                                Id = 1,
                                BeginTime = new DateTime(2021, 4, 15, 8, 00, 00),
                                EndTime = new DateTime(2021, 4, 15, 17, 00, 00)
                            }
                        }
                    }
                },
                {
                    2,
                    new WorkDay()
                    {
                        Id = 2,
                        ProducerId = usersDictionary["nail_maker"].Id,
                        WorkDaysTimeSpans = new List<WorkDaysTimeSpan>()
                        {
                            new WorkDaysTimeSpan()
                            {
                                Id = 3,
                                BeginTime = new DateTime(2021, 4, 11, 10, 00, 00),
                                EndTime = new DateTime(2021, 4, 11, 20, 00, 00)
                            }
                        }
                    }
                }
            };

            categoriesDictionary = new Dictionary<int, ServicesCategory>()
            {
                {
                    0,
                    new ServicesCategory() { Id = 0, Name = "Автомобиль" }
                },
                {
                    1,
                    new ServicesCategory() { Id = 1, Name = "Красота" }
                },
                {
                    2,
                    new ServicesCategory() { Id = 2, Name = "Компьютер" }
                }
            };

            addressesDictionary = new Dictionary<int, Address>()
            {
                {
                    0,
                    new Address(){ Id = 0, AddressValue = "ул.Ленина, д.100" }
                },
                {
                    1,
                    new Address(){ Id = 1, AddressValue = "ул.Полярная, д.89, кв.45" }
                },
                {
                    2,
                    new Address(){ Id = 2, AddressValue = "ул.1-Мая, д.29, кв.15" }
                }
            };

            Services = new List<Service>()
            {
                new Service()
                {
                    Id = 0,
                    ProducerId = usersDictionary["car_washer"].Id,
                    Appointments = new List<Appointment>()
                    {
                        new Appointment()
                        {
                            Id = 0,
                            Address = addressesDictionary[0],
                            Time = new DateTime(2021, 4, 15, 12, 00, 00),
                            WorkDay = workDays[0]
                        },
                        new Appointment()
                        {
                            Id = 1,
                            Address = addressesDictionary[0],
                            Time = new DateTime(2021, 4, 15, 12, 30, 00),
                            WorkDay = workDays[0]
                        }
                    },
                    Duration = 30,
                    Name = "Мойка снаружи",
                    Price = 200,
                    Category = categoriesDictionary[0]
                },
                new Service()
                {
                    Id = 1,
                    ProducerId = usersDictionary["car_washer"].Id,
                    Appointments = new List<Appointment>()
                    {
                        new Appointment()
                        {
                            Id = 2,
                            Address = addressesDictionary[0],
                            Time = new DateTime(2021, 4, 16, 10, 00, 00),
                            WorkDay = workDays[0]
                        }
                    },
                    Duration = 90,
                    Name = "Мойка комплекс",
                    Price = 700,
                    Category = categoriesDictionary[0]
                },
                new Service()
                {
                    Id = 2,
                    ProducerId = usersDictionary["barber"].Id,
                    Appointments = new List<Appointment>()
                    {
                        new Appointment()
                        {
                            Id = 3,
                            Address = addressesDictionary[1],
                            Time = new DateTime(2021, 4, 14, 8, 00, 00),
                            WorkDay = workDays[1]
                        }
                    },
                    Duration = 30,
                    Name = "Модельная стрижка",
                    Price = 400,
                    Category = categoriesDictionary[1]
                },
                new Service()
                {
                    Id = 3,
                    ProducerId = usersDictionary["nail_maker"].Id,
                    Appointments = new List<Appointment>()
                    {
                        new Appointment()
                        {
                            Id = 4,
                            Address = addressesDictionary[2],
                            Time = new DateTime(2021, 4, 11, 10, 00, 00),
                            WorkDay = workDays[1]
                        },
                        new Appointment()
                        {
                            Id = 5,
                            Address = addressesDictionary[2],
                            Time = new DateTime(2021, 4, 11, 11, 00, 00),
                            WorkDay = workDays[1]
                        },
                        new Appointment()
                        {
                            Id = 6,
                            Address = addressesDictionary[2],
                            Time = new DateTime(2021, 4, 11, 12, 00, 00),
                            WorkDay = workDays[1]
                        }
                    },
                    Duration = 60,
                    Name = "Наращивание ногтей",
                    Price = 500,
                    Category = categoriesDictionary[1]
                }

            }.AsQueryable();
        }

        // Словарь пользователей
        private static Dictionary<string, User> usersDictionary;

        // Словарь рабочих дней
        private Dictionary<int, WorkDay> workDays;

        // Словарь категорий
        private Dictionary<int, ServicesCategory> categoriesDictionary;

        // Словарь адресов
        private Dictionary<int, Address> addressesDictionary;

        // Коллекция данных для БД
        public IQueryable<Service> Services { get; private set; }

        // Метод для заполнения БД тестовыми данными
        public void PopulateDBWithFakeData(IServiceProvider services)
        {
            AppointmentJournalContext context = services.GetRequiredService<AppointmentJournalContext>();

            if (!context.Services.Any())
            {
                context.Services.AddRange(
                   Services
                );
                context.SaveChanges();
            }
        }

        // Список тестовых пользователей
        private static List<(string name, string email, string password, List<string> roles)> Users = new List<(string name, string email, string password, List<string> roles)>()
        {
            ("user1", "user1@email.com", "@Qaz123usr", new List<string> (){ Constants.ConsumersRole }),
            ("user2", "user2@email.com", "@Qaz123usr", new List<string> (){ Constants.ConsumersRole }),
            ("user3", "user3@email.com", "@Qaz123usr", new List<string> (){ Constants.ConsumersRole }),

            ("car_washer", "car_washer@email.com", "@Qaz123usr", new List<string> (){ Constants.ConsumersRole, Constants.ProducersRole }),
            ("barber", "barber2@email.com", "@Qaz123usr", new List<string> (){ Constants.ConsumersRole, Constants.ProducersRole }),
            ("nail_maker", "nail_maker@email.com", "@Qaz123usr", new List<string> (){ Constants.ConsumersRole, Constants.ProducersRole })
        };

        // Метод для создания тестовых пользователей в случае использования dependency injection
        public void CreateFakeUsers()
        {
            usersDictionary = new Dictionary<string, User>();

            int counter = 0;

            foreach (var user in Users)
            {
                var addedUser = new User { Id = "id" + counter, Email = user.email, UserName = user.name };

                usersDictionary.Add(user.name, addedUser);

                counter++;
            }
        }

        // Метод для создания тестовых пользователей
        public static async Task CreateFakeUsersInDB(IServiceProvider serviceProvider)
        {
            UserManager<User> userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            usersDictionary = new Dictionary<string, User>();

            foreach (var user in Users)
            {
                if (await userManager.FindByNameAsync(user.name) == null)
                {
                    foreach (var role in user.roles)
                    {
                        if (await roleManager.FindByNameAsync(role) == null)
                        {
                            await roleManager.CreateAsync(new IdentityRole(role));
                        }
                    }

                    User new_user = new User
                    {
                        UserName = user.name,
                        Email = user.email
                    };

                    IdentityResult result = await userManager.CreateAsync(new_user, user.password);

                    if (result.Succeeded)
                    {
                        foreach (var role in user.roles)
                        {
                            await userManager.AddToRoleAsync(new_user, role);
                        }
                    }

                    var addedUser = await userManager.FindByNameAsync(user.name);

                    usersDictionary.Add(user.name, addedUser);
                }
                else
                {
                    var addedUser = await userManager.FindByNameAsync(user.name);

                    usersDictionary.Add(user.name, addedUser);
                }
            }
        }
    }
}
