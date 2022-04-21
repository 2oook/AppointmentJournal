using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentJournal.AppDatabase;

/// <summary>
/// Provider for test DB data filling
/// </summary>
public class FakeDataForDBProvider
{
    public FakeDataForDBProvider(IServiceProvider serviceProvider)
    {
        CreateFakeUsersInDB(serviceProvider).Wait();

        var addressesDictionary = new Dictionary<int, Address>()
        {
            {
                0,
                new Address(){ AddressValue = "ул.Ленина, д.100", ProducerId = usersDictionary["car_washer"].Id, }
            },
            {
                1,
                new Address(){ AddressValue = "ул.Полярная, д.89, кв.45", ProducerId = usersDictionary["barber"].Id, }
            },
            {
                2,
                new Address(){ AddressValue = "ул.1-Мая, д.29, кв.15", ProducerId = usersDictionary["nail_maker"].Id, }
            }
        };

        var categoriesDictionary = new Dictionary<int, ServicesCategory>()
        {
            {
                0,
                new ServicesCategory() { Name = "Автомобиль" }
            },
            {
                1,
                new ServicesCategory() { Name = "Красота" }
            },
            {
                2,
                new ServicesCategory() { Name = "Компьютер" }
            }
        };

        var servicesDictionary = new Dictionary<int, Service>()
        {
            {
                0,
                new Service()
                {
                    Producer = usersDictionary["car_washer"],
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
                    Producer =usersDictionary["car_washer"],
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
                    Producer =usersDictionary["barber"],
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
                    Producer =usersDictionary["nail_maker"],
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
                    Address = addressesDictionary[0],
                    BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(8).Month, DateTime.Now.AddDays(8).Day, 10, 00, 00),
                    EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(8).Month, DateTime.Now.AddDays(8).Day, 15, 00, 00),
                    Service = servicesDictionary[0],
                }
            },
            {
                1,
                new WorkDaysTimeSpan()
                {
                    Address = addressesDictionary[1],
                    BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(8).Month, DateTime.Now.AddDays(8).Day, 15, 00, 00),
                    EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(8).Month, DateTime.Now.AddDays(8).Day, 19, 00, 00),
                    Service = servicesDictionary[1],
                }
            },
            {
                2,
                new WorkDaysTimeSpan()
                {
                    Address = addressesDictionary[2],
                    BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(10).Month, DateTime.Now.AddDays(10).Day, 8, 00, 00),
                    EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(10).Month, DateTime.Now.AddDays(10).Day, 17, 00, 00),
                    Service = servicesDictionary[2],
                }
            },
            {
                3,
                new WorkDaysTimeSpan()
                {
                    Address = addressesDictionary[2],
                    BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(11).Month, DateTime.Now.AddDays(11).Day, 10, 00, 00),
                    EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(11).Month, DateTime.Now.AddDays(11).Day, 20, 00, 00),
                    Service = servicesDictionary[3],
                }
            }
        };

        var workDays = new Dictionary<int, WorkDay>()
        {
            {
                0,
                new WorkDay()
                {
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

        var appointmentsDictionary = new Dictionary<int, Appointment>()
        {
            {
                0,
                new Appointment()
                {
                    Time = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(8).Month, DateTime.Now.AddDays(8).Day, 12, 00, 00),
                    WorkDayTimeSpan = workDaysTimeSpans[0],
                    ConsumerId = usersDictionary["car_washer"].Id
                }
            },
            {
                1,
                new Appointment()
                {
                    Time = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(8).Month, DateTime.Now.AddDays(8).Day, 12, 30, 00),
                    WorkDayTimeSpan = workDaysTimeSpans[0],
                    ConsumerId = usersDictionary["car_washer"].Id
                }
            },
            {
                2,
                new Appointment()
                {
                    Time = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(8).Month, DateTime.Now.AddDays(8).Day, 17, 00, 00),
                    WorkDayTimeSpan = workDaysTimeSpans[1],
                    ConsumerId = usersDictionary["car_washer"].Id
                }
            },
            {
                3,
                new Appointment()
                {
                    Time = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(10).Month, DateTime.Now.AddDays(10).Day, 8, 00, 00),
                    WorkDayTimeSpan = workDaysTimeSpans[2],
                    ConsumerId = usersDictionary["car_washer"].Id
                }
            },
            {
                4,
                new Appointment()
                {
                    Time = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(11).Month, DateTime.Now.AddDays(11).Day, 10, 00, 00),
                    WorkDayTimeSpan = workDaysTimeSpans[3],
                    ConsumerId = usersDictionary["car_washer"].Id
                }
            },
            {
                5,
                new Appointment()
                {
                    Time = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(11).Month, DateTime.Now.AddDays(11).Day, 11, 00, 00),
                    WorkDayTimeSpan = workDaysTimeSpans[3],
                    ConsumerId = usersDictionary["car_washer"].Id
                }
            },
            {
                6,
                new Appointment()
                {
                    Time = new DateTime(DateTime.Now.Year, DateTime.Now.AddDays(11).Month, DateTime.Now.AddDays(11).Day, 12, 00, 00),
                    WorkDayTimeSpan = workDaysTimeSpans[3],
                    ConsumerId = usersDictionary["car_washer"].Id
                }
            }
        };

        // установка ссылок на рабочие дни
        workDaysTimeSpans[0].WorkDay = workDays[0];
        workDaysTimeSpans[1].WorkDay = workDays[0];
        workDaysTimeSpans[2].WorkDay = workDays[1];
        workDaysTimeSpans[3].WorkDay = workDays[2];

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
        WorkDays = workDays.Values.AsQueryable();
    }

    // Словарь пользователей
    private static Dictionary<string, User> usersDictionary = new Dictionary<string, User>();

    // Коллекция сервисов для БД
    public IQueryable<Service> Services { get; private set; }

    // Коллекция рабочих дней
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
        ("user1", "user1@email.com", "@Qaz123usr", new List<string> (){ DatabaseConstants.ConsumersRole }),
        ("user2", "user2@email.com", "@Qaz123usr", new List<string> (){ DatabaseConstants.ConsumersRole }),
        ("user3", "user3@email.com", "@Qaz123usr", new List<string> (){ DatabaseConstants.ConsumersRole }),

        ("car_washer", "car_washer@email.com", "@Qaz123usr", new List<string> (){ DatabaseConstants.ConsumersRole, DatabaseConstants.ProducersRole }),
        ("barber", "barber2@email.com", "@Qaz123usr", new List<string> (){ DatabaseConstants.ConsumersRole, DatabaseConstants.ProducersRole }),
        ("nail_maker", "nail_maker@email.com", "@Qaz123usr", new List<string> (){ DatabaseConstants.ConsumersRole, DatabaseConstants.ProducersRole })
    };

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