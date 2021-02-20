using AppointmentJournal.AppReversedDatabase;
using AppointmentJournal.Models;
using AppointmentJournal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

            var producersServices = _serviceRepository.Services.Where(x => x.ProducerId == userId).ToList();

            var manageAppointmentsViewModel = new ManageAppointmentsViewModel() 
            {
                ServicesList = producersServices
            };

            return View(manageAppointmentsViewModel);
        }

        [HttpPost]
        public IActionResult AddService(AddServiceViewModel addServiceViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // TODO проверка полей сервиса 

                var userId = _userManager.GetUserId(User);

                addServiceViewModel.Service.ProducerId = userId;
                addServiceViewModel.Service.Category = addServiceViewModel.Category;

                var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

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
        public IActionResult RemoveService(long serviceId) 
        {
            var userId = _userManager.GetUserId(User);

            var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

            var service = _serviceRepository.Services.SingleOrDefault(s => s.Id == serviceId);

            context.Services.Remove(service);

            context.SaveChanges();

            return RedirectToAction(nameof(ManageAppointments));
        }

        public ViewResult Index()
        {
            return View();
        }
    }
}
