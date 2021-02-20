﻿using AppointmentJournal.Models;
using AppointmentJournal.Other;
using AppointmentJournal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AppointmentJournal.AppReversedDatabase;
using Microsoft.Extensions.DependencyInjection;
using AppointmentJournal.Infrastructure;

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

        public ViewResult Index() 
        {
            return View();
        }

        // Метод для выбора дня для записи  
        public ViewResult ChooseDay(int serviceId)
        {
            var service = _serviceRepository.Services.SingleOrDefault(s => s.Id == serviceId);
            var serviceProducerWorkDays = _serviceRepository.WorkDays.Where(wd => wd.ProducerId == service.ProducerId).ToList();

            var dates = DateTimePicker.CreateFourWeeksCalendar(serviceProducerWorkDays);

            var bookViewModel = new BookAppointmentViewModel()
            {
                Dates = dates,
                ServiceId = serviceId
            };

            return View(bookViewModel);
        }

        // Метод для выбора времени записи
        public ViewResult ChooseTime(int serviceId, DateTime chosenDate)
        {
            var service = _serviceRepository.Services.Include(x => x.WorkDaysTimeSpans).ThenInclude(x => x.WorkDay).Include(x => x.Appointments).SingleOrDefault(s => s.Id == serviceId);
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
        public RedirectResult Book(int serviceId, DateTime chosenTime, string returnUrl = "/") 
        {
            var userId = _userManager.GetUserId(User);

            var service = _serviceRepository.Services
                .Include(x => x.WorkDaysTimeSpans).ThenInclude(x => x.WorkDay)
                .Include(x => x.Appointments).ThenInclude(x => x.Address)
                .SingleOrDefault(s => s.Id == serviceId);
            var timeSpansForChosenDay = service.WorkDaysTimeSpans.Where(ts => ts.WorkDay.Date.Date == chosenTime.Date.Date).ToList();

            var appointment = new Appointment()
            {
                Address = service.Appointments.First().Address,
                Time = chosenTime,
                WorkDayTimeSpan = timeSpansForChosenDay.First()
            };

            service.Appointments.Add(appointment);

            var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

            context.SaveChanges();

            return Redirect(returnUrl);
        }
    }
}
