using AppointmentJournal.AppReversedDatabase;
using AppointmentJournal.Models;
using AppointmentJournal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private IServiceRepository _serviceRepository;
        private readonly UserManager<User> _userManager;

        public ProducerController(IServiceProvider services, IServiceRepository serviceRepository, UserManager<User> userManager)
        {
            _serviceProvider = services;
            _serviceRepository = serviceRepository;
            _userManager = userManager;
        }

        public ViewResult ManageAppointments() 
        {
            var userId = _userManager.GetUserId(User);

            var producersServices = _serviceRepository.Services.Include(x => x.Category).Where(x => x.ProducerId == userId).ToList();

            var manageAppointmentsViewModel = new ManageAppointmentsViewModel() 
            {
                ServicesList = producersServices
            };

            return View(manageAppointmentsViewModel);
        }

        [HttpPost]
        public IActionResult AddService(ServiceViewModel addServiceViewModel, string returnUrl)
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

                return RedirectToAction(nameof(ManageAppointments));
            }

            return View(addServiceViewModel);
        }

        [HttpGet]
        public ViewResult AddService()
        {
            return View();
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

                return RedirectToAction(nameof(ManageAppointments));
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
        public ViewResult EditService(long serviceId)
        {
            var service = _serviceRepository.Services.Include(x => x.Category).SingleOrDefault(s => s.Id == serviceId);

            var serviceViewModel = new ServiceViewModel() 
            {
                Service = service,
                Category = service.Category
            };

            return View(serviceViewModel);
        }

        [HttpPost]
        public IActionResult RemoveService(long serviceId) 
        {
            try
            {
                var userId = _userManager.GetUserId(User);

                var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

                var service = _serviceRepository.Services.SingleOrDefault(s => s.Id == serviceId & s.ProducerId == userId);
                context.Services.Remove(service);
                context.SaveChanges();
            }
            catch
            {
                // TODO конкретизировать ошибку
                return View("Error", "Невозможно удалить услугу");
            }

            return RedirectToAction(nameof(ManageAppointments));
        }
    }
}
