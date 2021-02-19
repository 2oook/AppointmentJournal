using AppointmentJournal.Models;
using AppointmentJournal.Other;
using AppointmentJournal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AppointmentJournal.Controllers
{
    [Authorize(Roles = Constants.ConsumersRole + "," + Constants.ProducersRole)]
    public class ConsumerController : Controller
    {
        private IServiceRepository serviceRepository;

        public ConsumerController(IServiceRepository serviceRepository)
        {
            this.serviceRepository = serviceRepository;
        }

        public ViewResult Index() 
        {
            return View();
        }

        // Метод для выбора дня для записи  
        public ViewResult ChooseDay(int serviceId)
        {
            var service = serviceRepository.Services.SingleOrDefault(s => s.Id == serviceId);
            var serviceProducerWorkDays = serviceRepository.WorkDays.Where(wd => wd.ProducerId == service.ProducerId).ToList();

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
            var service = serviceRepository.Services.Include(x => x.WorkDaysTimeSpans).ThenInclude(x => x.WorkDay).Include(x => x.Appointments).SingleOrDefault(s => s.Id == serviceId);
            var timeSpansForChosenDay = service.WorkDaysTimeSpans.Where(ts => ts.WorkDay.Date.Date == chosenDate.Date.Date).ToList();

            var appointmentAvailableTimeList = DateTimePicker.CreateAppointmentAvailableTimeList(service.Duration, timeSpansForChosenDay);

            var chooseTimeViewModel = new ChooseTimeViewModel()
            {
                AppointmentTimesList = appointmentAvailableTimeList
            };

            return View(chooseTimeViewModel);
        }

        // Метод для бронирования времени 
        public ViewResult Book(DateTime chosenTime) 
        {
            return View();
        }
    }
}
