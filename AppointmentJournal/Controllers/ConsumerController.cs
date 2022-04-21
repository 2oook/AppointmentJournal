using AppointmentJournal.Other;
using AppointmentJournal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AppointmentJournal.AppDatabase;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AppointmentJournal.Controllers
{
    /// <summary>
    /// Контроллер для обработки запросов связанных с действиями потребителя
    /// </summary>
    [Authorize(Roles = DatabaseConstants.ConsumersRole + "," + DatabaseConstants.ProducersRole)]
    public class ConsumerController : Controller
    {
        private IServiceProvider _serviceProvider;
        private readonly UserManager<User> _userManager;

        public ConsumerController(IServiceProvider services, UserManager<User> userManager)
        {
            _serviceProvider = services;
            _userManager = userManager;
        }

        /// <summary>
        /// Метод для выбора дня для записи  
        /// </summary>
        /// <param name="serviceId">ID услуги</param>
        /// <returns></returns>
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

        /// <summary>
        ///  Метод для выбора времени записи
        /// </summary>
        /// <param name="serviceId">ID услуги</param>
        /// <param name="chosenDate">Выбранная дата</param>
        /// <returns></returns>
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

        /// <summary>
        /// Метод для бронирования
        /// </summary>
        /// <param name="serviceId">ID услуги</param>
        /// <param name="chosenTime">Выбранное время</param>
        /// <returns></returns>
        public ViewResult Book(long serviceId, DateTime chosenTime) 
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

        /// <summary>
        ///  Метод для удаления записи бронирования
        /// </summary>
        /// <param name="appointmentId">ID записи бронирования</param>
        /// <param name="returnUrl">Url для возврата</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RemoveAppointment(long appointmentId, string returnUrl = "/") 
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

        /// <summary>
        /// Метод для получения списка бронирований для данного пользователя
        /// </summary>
        /// <returns></returns>
        public ViewResult ManageAppointments() 
        {
            var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

            var userId = _userManager.GetUserId(User);

            var appointments = context.Appointments
                .Include(x => x.Service)
                .ThenInclude(x => x.Category)
                .Include(x => x.WorkDayTimeSpan)
                .ThenInclude(x => x.Address)
                .Where(x => x.ConsumerId == userId).AsEnumerable().Select(async x =>
                {
                    x.Service.Producer = await _userManager.FindByIdAsync(x.Service.ProducerId);
                    return x;
                })
                .Select(x => x.Result).ToList();

            var model = new ManageAppointmentsViewModel()
            {
                Appointments = appointments
            };

            return View(model);
        }
    }
}
