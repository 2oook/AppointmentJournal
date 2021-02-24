using AppointmentJournal.AppReversedDatabase;
using AppointmentJournal.Models;
using AppointmentJournal.Other;
using AppointmentJournal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.Controllers
{
    [Authorize(Roles = Constants.ProducersRole)]
    public class ProducerController : Controller
    {
        private IServiceProvider _serviceProvider;
        private readonly UserManager<User> _userManager;

        public ProducerController(IServiceProvider services, UserManager<User> userManager)
        {
            _serviceProvider = services;
            _userManager = userManager;
        }

        public ViewResult ManageServices() 
        {
            var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

            var userId = _userManager.GetUserId(User);

            var producersServices = context.Services.Include(x => x.Category).Where(x => x.ProducerId == userId).ToList();

            var manageAppointmentsViewModel = new ManageServicesViewModel() 
            {
                ServicesList = producersServices
            };

            return View(manageAppointmentsViewModel);
        }

        public ViewResult ManageAddresses() 
        {
            var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

            var userId = _userManager.GetUserId(User);

            var addressesList = context.Addresses.Where(x => x.ProducerId == userId).ToList();

            var model = new ManageAddressesViewModel() 
            {
                AddressList = addressesList
            };

            return View(model);
        }

        [HttpGet]
        public ViewResult EditAddress(long addressId, string returnUrl)
        {
            var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

            var userId = _userManager.GetUserId(User);
            var address = context.Addresses.SingleOrDefault(x => x.ProducerId == userId & x.Id == addressId);

            var model = new AddressViewModel() 
            {
                Address = address,
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditAddress(AddressViewModel addressViewModel)
        {
            if (ModelState.IsValid)
            {
                // TODO проверка полей сервиса 

                var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

                var userId = _userManager.GetUserId(User);
                var address = context.Addresses.Include(x => x.WorkDaysTimeSpans).SingleOrDefault(x => x.ProducerId == userId & x.Id == addressViewModel.Address.Id);

                address.AddressValue = addressViewModel.Address.AddressValue;

                context.SaveChanges();

                return Redirect(addressViewModel.ReturnUrl);
            }

            return View(addressViewModel);
        }

        [HttpPost]
        public IActionResult RemoveAddress(long addressId)
        {
            try
            {
                var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

                var userId = _userManager.GetUserId(User);
                var address = context.Addresses.Include(x => x.WorkDaysTimeSpans).SingleOrDefault(x => x.ProducerId == userId & x.Id == addressId);

                if (address.WorkDaysTimeSpans.Count > 0)
                {
                    throw new Exception("С данным адресом связаны один или несколько периодов рабочего времени");
                }

                context.Addresses.Remove(address);
                context.SaveChanges();
            }
            catch
            {
                // TODO конкретизировать ошибку
                return View("Error", "Невозможно удалить адрес");
            }

            return RedirectToAction(nameof(ManageAddresses));
        }

        [HttpGet]
        public ViewResult AddAddress(string returnUrl)
        {
            var model = new AddressViewModel() 
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult AddAddress(AddressViewModel addAddressViewModel)
        {
            if (ModelState.IsValid)
            {
                // TODO проверка полей сервиса 

                var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();
                var userId = _userManager.GetUserId(User);

                addAddressViewModel.Address.ProducerId = userId;

                context.Addresses.Add(addAddressViewModel.Address);
                context.SaveChanges();

                return Redirect(addAddressViewModel.ReturnUrl);
            }

            return View(addAddressViewModel);
        }

        [HttpPost]
        public IActionResult AddService(ServiceViewModel addServiceViewModel)
        {
            if (ModelState.IsValid)
            {
                // TODO проверка полей сервиса 

                var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

                var userId = _userManager.GetUserId(User);

                addServiceViewModel.Service.ProducerId = userId;

                ResolveCategoryAdding(context.ServicesCategories.ToList(), addServiceViewModel.Service, addServiceViewModel.Category);

                context.Services.Add(addServiceViewModel.Service);

                context.SaveChanges();

                return RedirectToAction(nameof(ManageServices));
            }

            return View(addServiceViewModel);
        }

        [HttpGet]
        public ViewResult AddService()
        {
            return View();
        }

        [HttpGet]
        public ViewResult EditService(long serviceId)
        {
            var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

            var service = context.Services.Include(x => x.Category).SingleOrDefault(s => s.Id == serviceId);

            var serviceViewModel = new ServiceViewModel()
            {
                Service = service,
                Category = service.Category
            };

            return View(serviceViewModel);
        }

        [HttpPost]
        public IActionResult EditService(ServiceViewModel serviceViewModel, long serviceId)
        {
            if (ModelState.IsValid)
            {
                // TODO проверка полей сервиса 

                var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

                var userId = _userManager.GetUserId(User);

                var service = context.Services.Include(x => x.Category).Include(x => x.Appointments).SingleOrDefault(s => s.Id == serviceId & s.ProducerId == userId);

                // если категория была изменена
                if (service.Category.Name != serviceViewModel.Category.Name)
                {
                    ResolveCategoryAdding(context.ServicesCategories.ToList(), service, serviceViewModel.Category);
                }

                service.Name = serviceViewModel.Service.Name;
                service.Price = serviceViewModel.Service.Price;

                // если время изменилось
                if (service.Duration != serviceViewModel.Service.Duration)
                {
                    if (service.Appointments.Count == 0)
                    {
                        service.Duration = serviceViewModel.Service.Duration;
                    }
                    else
                    {
                        return View("Error", "Невозможно изменить длительность услуги, так как на данную услугу существуют активные записи");
                    }
                }
                
                context.SaveChanges();

                return RedirectToAction(nameof(ManageServices));
            }

            return View(serviceViewModel);
        }

        /// <summary>
        /// Метод для принятия решения по добавлению новой категории услуг
        /// </summary>
        /// <param name="servicesCategories">Список всех категорий услуг</param>
        /// <param name="service">Услуга для которой устанавливается категория</param>
        /// <param name="servicesCategory">Категория</param>
        void ResolveCategoryAdding(List<ServicesCategory> servicesCategories, Service service, ServicesCategory servicesCategory) 
        {
            var existingCategory = servicesCategories.SingleOrDefault(cat => cat.Name == servicesCategory.Name);

            // если заданной категории нет в БД - добавить, если есть - ассоциировать с существующей категорией
            if (existingCategory == null)
            {
                service.Category = servicesCategory;
            }
            else
            {
                service.Category = existingCategory;
            }
        }

        [HttpGet]
        public ViewResult ManageService(long serviceId)
        {
            var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

            var service = context.Services.SingleOrDefault(s => s.Id == serviceId);
            var serviceProducerWorkDays = context.WorkDays
                .Where(wd => wd.ProducerId == service.ProducerId && wd.WorkDaysTimeSpans
                .Select(x => x.Service).Contains(service)).ToList();

            var dates = DateTimePicker.CreateFourWeeksCalendar(serviceProducerWorkDays);

            var bookViewModel = new BookAppointmentViewModel()
            {
                Dates = dates,
                ServiceId = serviceId,
                ServiceName = service.Name
            };

            return View(bookViewModel);
        }

        [HttpGet]
        public ViewResult ManageWorkDay(long serviceId, long chosenDateInTicks)
        {
            var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

            var chosenDate = new DateTime(chosenDateInTicks);

            var service = context.Services
                .Include(x => x.WorkDaysTimeSpans)
                .ThenInclude(x => x.WorkDay)
                .Include(x => x.WorkDaysTimeSpans)
                .ThenInclude(x => x.Address)
                .Include(x => x.Appointments).SingleOrDefault(s => s.Id == serviceId);

            var timeSpansForChosenDay = service.WorkDaysTimeSpans.Where(ts => ts.WorkDay.Date.Date == chosenDate.Date.Date).ToList();

            WorkDay workDay = null;

            if (timeSpansForChosenDay.Count != 0)
            {
                workDay = timeSpansForChosenDay.GroupBy(x => x.WorkDay).SingleOrDefault()?.Key;

                if (workDay == null)
                {
                    throw new Exception("workDay = null");
                }
            }

            var appointmentAvailableTimeList = DateTimePicker.CreateAppointmentAvailableTimeList(service.Duration, timeSpansForChosenDay);

            var chooseTimeViewModel = new ManageWorkDayViewModel()
            {
                AppointmentTimesList = appointmentAvailableTimeList,
                ServiceId = serviceId,
                ChosenDate = chosenDate,
                WorkDay = workDay
            };

            return View(chooseTimeViewModel);
        }

        [HttpPost]
        public IActionResult ToggleWorkDayAvailability(long workDayId, string returnUrl)
        {
            try
            {
                var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

                var workDay = context.WorkDays.SingleOrDefault(x => x.Id == workDayId);

                if (workDay.IsEnabled)
                {
                    workDay.IsEnabled = false;
                }
                else
                {
                    workDay.IsEnabled = true;
                }

                context.SaveChanges();
            }
            catch
            {
                // TODO конкретизировать ошибку
                return View("Error", "Невозможно сделать день доступным для записи");
            }

            return Redirect(returnUrl);
        }

        [HttpGet]
        public IActionResult AddWorkDaySpan(long serviceId, long chosenDateInTicks, string returnUrl)
        {
            var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();
            var userId = _userManager.GetUserId(User);
            var addresses = context.Addresses.Where(x => x.ProducerId == userId).Select(x => new SelectListItem { Value = x.Id.ToString() , Text = x.AddressValue }).ToList();

            var chosenDate = new DateTime(chosenDateInTicks);

            var model = new AddWorkDaySpanViewModel()
            {
                 ChosenDate = chosenDate,
                 ServiceId = serviceId,
                 Addresses = addresses,
                 ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult AddWorkDaySpan(AddWorkDaySpanViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.BeginHour >= model.EndHour)
                {
                    ModelState.AddModelError("", "Время конца периода должно быть не меньше времени начала");
                    return View(model);
                }

                var modelBeginTime = new DateTime(model.ChosenDate.Year, model.ChosenDate.Month, model.ChosenDate.Day, model.BeginHour, 0, 0);
                var modelEndTime = new DateTime(model.ChosenDate.Year, model.ChosenDate.Month, model.ChosenDate.Day, model.EndHour, 0, 0);

                var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

                var userId = _userManager.GetUserId(User);
                var service = context.Services
                    .Include(x => x.WorkDaysTimeSpans)
                    .ThenInclude(x => x.WorkDay).SingleOrDefault(s => s.Id == model.ServiceId & s.ProducerId == userId);

                // периоды времени для заданного сервиса и выбранного дня
                var timeSpansForChosenDay = service.WorkDaysTimeSpans.Where(ts => ts.WorkDay.Date.Date == model.ChosenDate.Date.Date).ToList();

                WorkDay workDay = null;

                if (timeSpansForChosenDay.Count == 0)
                {
                    workDay = new WorkDay()
                    {
                        Date = model.ChosenDate,
                        IsEnabled = false,
                        ProducerId = userId
                    };
                }
                else
                {
                    workDay = timeSpansForChosenDay.GroupBy(x => x.WorkDay).SingleOrDefault()?.Key;

                    if (workDay == null)
                    {
                        throw new Exception("workDay = null");
                    }

                    // проверка периодов рабочего времени на перекрытие
                    var overlapFlag = timeSpansForChosenDay.Any(x => 
                    (x.BeginTime.Hour <= modelBeginTime.Hour & x.EndTime.Hour >= modelBeginTime.Hour) || 
                    (x.BeginTime.Hour <= modelEndTime.Hour & x.EndTime.Hour >= modelEndTime.Hour));

                    if (overlapFlag)
                    {
                        ModelState.AddModelError("", "Заданный период перекрывает существующие периоды");
                        return View(model);
                    }
                }

                var address = context.Addresses.Include(x => x.WorkDaysTimeSpans).SingleOrDefault(x => x.ProducerId == userId & x.Id == model.AddressId);

                var workDayTimeSpan = new WorkDaysTimeSpan()
                {
                    BeginTime = modelBeginTime,
                    EndTime = modelEndTime,
                    WorkDay = workDay,
                    Address = address
                };

                service.WorkDaysTimeSpans.Add(workDayTimeSpan);

                context.SaveChanges();
            }

            return Redirect(model.ReturnUrl);
        }

        [HttpPost]
        public IActionResult RemoveWorkDayTimeSpan(long serviceId, long workDayTimeSpanId, string returnUrl) 
        {
            try
            {
                var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

                var userId = _userManager.GetUserId(User);
                var service = context.Services.Include(x => x.WorkDaysTimeSpans).SingleOrDefault(s => s.Id == serviceId & s.ProducerId == userId);

                var workDayTimeSpan = service.WorkDaysTimeSpans.SingleOrDefault(ts => ts.Id == workDayTimeSpanId);

                service.WorkDaysTimeSpans.Remove(workDayTimeSpan);

                context.SaveChanges();
            }
            catch
            {
                // TODO конкретизировать ошибку
                return View("Error", "Невозможно удалить период рабочего времени");
            }

            return Redirect(returnUrl);
        }

        [HttpPost]
        public IActionResult RemoveService(long serviceId) 
        {
            try
            {
                var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

                var userId = _userManager.GetUserId(User);
                var service = context.Services.SingleOrDefault(s => s.Id == serviceId & s.ProducerId == userId);

                context.Services.Remove(service);
                context.SaveChanges();
            }
            catch
            {
                // TODO конкретизировать ошибку
                return View("Error", "Невозможно удалить услугу");
            }

            return RedirectToAction(nameof(ManageServices));
        }
    }
}
