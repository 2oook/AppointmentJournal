using AppointmentJournal.Models;
using AppointmentJournal.Other;
using AppointmentJournal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public ViewResult Book(int serviceId)
        {
            var service = serviceRepository.Services.SingleOrDefault(s => s.Id == serviceId);

            if (service == null)
            {
                return View();
            }

            var dates = DateTimePicker.GetFourWeeks();

            var bookViewModel = new BookAppointmentViewModel()
            {
                Dates = dates
            };

            return View(bookViewModel);
        }

        public ViewResult ChooseDate(DateTime chosenDate)
        {
            return View();
        }
    }
}
