﻿using AppointmentJournal.Models;
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
using System.Collections.Generic;

namespace AppointmentJournal.Controllers
{
    [Authorize(Roles = Constants.ConsumersRole + "," + Constants.ProducersRole)]
    public class ConsumerController : Controller
    {
        private IServiceProvider _serviceProvider;
        private readonly UserManager<User> _userManager;

        public ConsumerController(IServiceProvider services, UserManager<User> userManager)
        {
            _serviceProvider = services;
            _userManager = userManager;
        }

        // Метод для выбора дня для записи  
        public ViewResult ChooseDay(long serviceId)
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

        // Метод для выбора времени записи
        public ViewResult ChooseTime(long serviceId, DateTime chosenDate)
        {
            var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

            var service = context.Services
                .Include(x => x.WorkDaysTimeSpans)
                .ThenInclude(x => x.WorkDay)
                .Include(x => x.WorkDaysTimeSpans)
                .ThenInclude(x => x.Address)
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
                var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

                var userId = _userManager.GetUserId(User);

                var service = context.Services
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
                    WorkDayTimeSpan = neededTimeSpan,
                    ConsumerId = userId
                };

                service.Appointments.Add(appointment);

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

        [HttpPost]
        public IActionResult RemoveAppointment(long appointmentId, string returnUrl) 
        {
            try
            {
                var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

                var userId = _userManager.GetUserId(User);

                var appointment = context.Appointments.SingleOrDefault(x => x.Id == appointmentId & x.ConsumerId == userId);

                context.Appointments.Remove(appointment);

                context.SaveChanges();
            }
            catch
            {
                // TODO конкретизировать ошибку
                return View("Error", "Невозможно удалить запись");
            }

            return Redirect(returnUrl);
        }

        public ViewResult ManageAppointments() 
        {
            var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

            var userId = _userManager.GetUserId(User);

            var appointments = context.Appointments
                .Include(x => x.Service)
                .ThenInclude(x => x.Category)
                .Include(x => x.WorkDayTimeSpan)
                .ThenInclude(x => x.Address)
                .Where(x => x.ConsumerId == userId).ToList();

            var model = new ManageAppointmentsViewModel()
            {
                Appointments = appointments
            };

            return View(model);
        }
    }
}
