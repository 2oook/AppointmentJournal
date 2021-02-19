﻿using AppointmentJournal.AppReversedDatabase;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.Models
{
    /// <summary>
    /// Фиктивное хранилище объектов предметной области (использовать для тестового наполнения бд)
    /// </summary>
    public class FakeServiceRepository : IServiceRepository
    {
        public FakeServiceRepository()
        {
            if (usersDictionary == null || usersDictionary.Count == 0)
            {
                CreateFakeUsers();
            }

            var addressesDictionary = new Dictionary<int, Address>()
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

            var categoriesDictionary = new Dictionary<int, ServicesCategory>()
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

            var servicesDictionary = new Dictionary<int, Service>()
            {
                {
                    0,
                    new Service()
                    {
                        Id = 0,
                        ProducerId = usersDictionary["car_washer"].Id,
                        Duration = 30,
                        Name = "Мойка снаружи",
                        Price = 200,
                        Category = categoriesDictionary[0],
                    }
                },
                {
                    1,
                    new Service()
                    {
                        Id = 1,
                        ProducerId = usersDictionary["car_washer"].Id,
                        Duration = 90,
                        Name = "Мойка комплекс",
                        Price = 700,
                        Category = categoriesDictionary[0]
                    }
                },
                {
                    2,
                    new Service()
                    {
                        Id = 2,
                        ProducerId = usersDictionary["barber"].Id,
                        Duration = 30,
                        Name = "Модельная стрижка",
                        Price = 400,
                        Category = categoriesDictionary[1]
                    }
                },
                {
                    3,
                    new Service()
                    {
                        Id = 3,
                        ProducerId = usersDictionary["nail_maker"].Id,
                        Duration = 60,
                        Name = "Наращивание ногтей",
                        Price = 500,
                        Category = categoriesDictionary[1]
                    }
                }

            };

            var workDaysTimeSpans = new Dictionary<int, WorkDaysTimeSpan>()
            {
                {
                    0,
                    new WorkDaysTimeSpan()
                    {
                        Id = 0,
                        BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(8).Month, DateTime.Now.AddDays(8).Day, 10, 00, 00),
                        EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(8).Month, DateTime.Now.AddDays(8).Day, 15, 00, 00),
                        Service = servicesDictionary[0],
                        ServiceId = 0,
                        WorkDayId = 0
                    }
                },
                {
                    1,
                    new WorkDaysTimeSpan()
                    {
                        Id = 1,
                        BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(8).Month, DateTime.Now.AddDays(8).Day, 15, 00, 00),
                        EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(8).Month, DateTime.Now.AddDays(8).Day, 19, 00, 00),
                        Service = servicesDictionary[1],
                        ServiceId = 1,
                        WorkDayId = 0
                    }
                },
                {
                    2,
                    new WorkDaysTimeSpan()
                    {
                        Id = 2,
                        BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(10).Month, DateTime.Now.AddDays(10).Day, 8, 00, 00),
                        EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(10).Month, DateTime.Now.AddDays(10).Day, 17, 00, 00),
                        Service = servicesDictionary[2],
                        ServiceId = 2,
                        WorkDayId = 1
                    }
                },
                {
                    3,
                    new WorkDaysTimeSpan()
                    {
                        Id = 3,
                        BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(11).Month, DateTime.Now.AddDays(11).Day, 10, 00, 00),
                        EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(11).Month, DateTime.Now.AddDays(11).Day, 20, 00, 00),
                        Service = servicesDictionary[3],
                        ServiceId = 3,
                        WorkDayId = 2
                    }
                }
            };

            var appointmentsDictionary = new Dictionary<int, Appointment>()
            {
                {
                    0,
                    new Appointment()
                    {
                        Id = 0,
                        Address = addressesDictionary[0],
                        Time = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(8).Month, DateTime.Now.AddDays(8).Day, 12, 00, 00),
                        WorkDayTimeSpan = workDaysTimeSpans[0]
                    }
                },
                {
                    1,
                    new Appointment()
                    {
                        Id = 1,
                        Address = addressesDictionary[0],
                        Time = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(8).Month, DateTime.Now.AddDays(8).Day, 12, 30, 00),
                        WorkDayTimeSpan = workDaysTimeSpans[0]
                    }
                },
                {
                    2,
                    new Appointment()
                    {
                        Id = 2,
                        Address = addressesDictionary[0],
                        Time = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(8).Month, DateTime.Now.AddDays(8).Day, 17, 00, 00),
                        WorkDayTimeSpan = workDaysTimeSpans[1]
                    }
                },
                {
                    3,
                    new Appointment()
                    {
                        Id = 3,
                        Address = addressesDictionary[1],
                        Time = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(10).Month, DateTime.Now.AddDays(10).Day, 8, 00, 00),
                        WorkDayTimeSpan = workDaysTimeSpans[2]
                    }
                },
                {
                    4,
                    new Appointment()
                    {
                        Id = 4,
                        Address = addressesDictionary[2],
                        Time = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(11).Month, DateTime.Now.AddDays(11).Day, 10, 00, 00),
                        WorkDayTimeSpan = workDaysTimeSpans[3]
                    }
                },
                {
                    5,
                    new Appointment()
                    {
                        Id = 5,
                        Address = addressesDictionary[2],
                        Time = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(11).Month, DateTime.Now.AddDays(11).Day, 11, 00, 00),
                        WorkDayTimeSpan = workDaysTimeSpans[3]
                    }
                },
                {
                    6,
                    new Appointment()
                    {
                        Id = 6,
                        Address = addressesDictionary[2],
                        Time = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(11).Month, DateTime.Now.AddDays(11).Day, 12, 00, 00),
                        WorkDayTimeSpan = workDaysTimeSpans[3]
                    }
                }
            };

            // установка ссылок на промежутки времени для сервисов
            servicesDictionary[0].WorkDaysTimeSpans = new List<WorkDaysTimeSpan>()
            {
                workDaysTimeSpans[0]
            };

            servicesDictionary[1].WorkDaysTimeSpans = new List<WorkDaysTimeSpan>()
            {
                workDaysTimeSpans[1]
            };

            servicesDictionary[2].WorkDaysTimeSpans = new List<WorkDaysTimeSpan>()
            {
                workDaysTimeSpans[2]
            };

            servicesDictionary[3].WorkDaysTimeSpans = new List<WorkDaysTimeSpan>()
            {
                workDaysTimeSpans[3]
            };

            // установка ссылок на встречи для сервисов
            servicesDictionary[0].Appointments = new List<Appointment>()
            {
                appointmentsDictionary[0],
                appointmentsDictionary[1]
            };

            servicesDictionary[1].Appointments = new List<Appointment>()
            {
                appointmentsDictionary[2]
            };

            servicesDictionary[2].Appointments = new List<Appointment>()
            {
                appointmentsDictionary[3]
            };

            servicesDictionary[3].Appointments = new List<Appointment>()
            {
                appointmentsDictionary[4],
                appointmentsDictionary[5],
                appointmentsDictionary[6]
            };

            Services = servicesDictionary.Values.AsQueryable();

            var workDays = new Dictionary<int, WorkDay>()
            {
                {
                    0,
                    new WorkDay()
                    {
                        Id = 0,
                        ProducerId = usersDictionary["car_washer"].Id,
                        WorkDaysTimeSpans = new List<WorkDaysTimeSpan>()
                        {
                            workDaysTimeSpans[0],
                            workDaysTimeSpans[1]
                        },
                        Date = DateTime.Now.AddDays(8),
                        IsEnabled = true
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
                            workDaysTimeSpans[2]
                        },
                        Date = DateTime.Now.AddDays(10),
                        IsEnabled = true
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
                            workDaysTimeSpans[3]
                        },
                        Date = DateTime.Now.AddDays(11),
                        IsEnabled = true
                    }
                }
            };

            WorkDays = workDays.Values.AsQueryable();
        }

        // Словарь пользователей
        private static Dictionary<string, User> usersDictionary;

        // Коллекция сервисов для БД
        public IQueryable<Service> Services { get; private set; }

        public IQueryable<WorkDay> WorkDays { get; private set; }

        // Метод для заполнения БД тестовыми данными
        public void PopulateDBWithFakeData(IServiceProvider services)
        {
            AppointmentJournalContext context = services.GetRequiredService<AppointmentJournalContext>();

            if (!context.Services.Any())
            {
                context.Services.AddRange(
                   Services
                );
                context.WorkDays.AddRange(
                   WorkDays
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
