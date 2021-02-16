using AppointmentJournal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.Controllers
{
    public class ConsumerController : Controller
    {
        private IServiceRepository serviceRepository;

        public ConsumerController(IServiceRepository serviceRepository)
        {
            this.serviceRepository = serviceRepository;
        }

        [Authorize(Roles = Constants.ConsumersRole + "," + Constants.ProducersRole)]
        public ViewResult Index() 
        {
            return View();
        }
    }
}
