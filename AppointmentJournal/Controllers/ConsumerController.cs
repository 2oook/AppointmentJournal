using AppointmentJournal.Models;
using AppointmentJournal.Other;
using AppointmentJournal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AppointmentJournal.AppReversedDatabase;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace AppointmentJournal.Controllers
{
    [Authorize(Roles = Constants.ConsumersRole + "," + Constants.ProducersRole)]
    public class ConsumerController : Controller
    {
        private IServiceProvider _serviceProvider;
        private IServiceRepository _serviceRepository;
        private readonly UserManager<User> _userManager;

        public ConsumerController(IServiceProvider services, IServiceRepository serviceRepository, UserManager<User> userManager)
        {
            _serviceProvider = services;
            _serviceRepository = serviceRepository;
            _userManager = userManager;
        }

        // Метод для выбора дня для записи  
        public ViewResult ChooseDay(long serviceId)
        {
            var service = _serviceRepository.Services.SingleOrDefault(s => s.Id == serviceId);
            var serviceProducerWorkDays = _serviceRepository.WorkDays
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

        // Метод для выбора времени записи
        public ViewResult ChooseTime(long serviceId, DateTime chosenDate)
        {
            var service = _serviceRepository.Services
                .Include(x => x.WorkDaysTimeSpans)
                .ThenInclude(x => x.WorkDay)
                .Include(x => x.Appointments).SingleOrDefault(s => s.Id == serviceId);

            var timeSpansForChosenDay = service.WorkDaysTimeSpans.Where(ts => ts.WorkDay.Date.Date == chosenDate.Date.Date).ToList();

            var appointmentAvailableTimeList = DateTimePicker.CreateAppointmentAvailableTimeList(service.Duration, timeSpansForChosenDay);

            var chooseTimeViewModel = new ChooseTimeViewModel()
            {
                AppointmentTimesList = appointmentAvailableTimeList,
                ServiceId = serviceId
            };

            return View(chooseTimeViewModel);
        }

        // Метод для бронирования времени 
        public ViewResult Book(long serviceId, DateTime chosenTime, string returnUrl = "/") 
        {
            try
            {
                var userId = _userManager.GetUserId(User);

                var service = _serviceRepository.Services
                    .Include(x => x.WorkDaysTimeSpans).ThenInclude(x => x.WorkDay)
                    .Include(x => x.WorkDaysTimeSpans).ThenInclude(x => x.Address)
                    .Include(x => x.Appointments)
                    .SingleOrDefault(s => s.Id == serviceId);

                var timeSpansForChosenDay = service.WorkDaysTimeSpans.Where(ts => ts.WorkDay.Date.Date == chosenTime.Date.Date).ToList();

                var neededTimeSpan = timeSpansForChosenDay.SingleOrDefault(x => (x.BeginTime.Hour <= chosenTime.Hour & x.EndTime.Hour >= chosenTime.Hour));

                if (neededTimeSpan == null)
                {
                    throw new Exception("neededTimeSpan = null");
                }

                var appointment = new Appointment()
                {
                    Time = chosenTime,
                    WorkDayTimeSpan = neededTimeSpan
                };

                service.Appointments.Add(appointment);

                var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

                context.SaveChanges();

                ViewBag.Result = true;

                return View();
            }
            catch
            {
                ViewBag.Result = false;

                return View();
            }           
        }
    }
}
