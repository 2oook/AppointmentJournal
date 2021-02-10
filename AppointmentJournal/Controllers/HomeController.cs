using AppointmentJournal.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.Controllers
{
    public class HomeController : Controller
    {
        private IServiceRepository serviceRepository;

        public HomeController(IServiceRepository serviceRepository)
        {
            this.serviceRepository = serviceRepository;
        }

        public ViewResult Index() 
        {
            return View();
        }
    }
}
