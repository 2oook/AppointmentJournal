﻿using AppointmentJournal.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.Controllers
{
    public class ProducerController : Controller
    {
        private IServiceRepository serviceRepository;

        public ProducerController(IServiceRepository serviceRepository)
        {
            this.serviceRepository = serviceRepository;
        }

        public ViewResult Index()
        {
            return View();
        }
    }
}
