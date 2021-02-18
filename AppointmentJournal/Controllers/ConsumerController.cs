using AppointmentJournal.Models;
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

        public ViewResult Book(int Id)
        {
            var serviceId = Id;

            var service = serviceRepository.Services.SingleOrDefault(s => s.Id == serviceId);

            if (service == null)
            {
                return View();
            }



            return View();
        }
    }
}
