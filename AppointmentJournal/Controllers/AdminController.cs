using AppointmentJournal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.Controllers
{
    [Authorize(Roles = "Admins")]
    public class AdminController : Controller
    {      
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SeedDatabaseWithFakeData()
        {          
            var fakeDataProvider = new FakeDataForDBProvider(HttpContext.RequestServices);

            fakeDataProvider.PopulateDBWithFakeData(HttpContext.RequestServices);

            return RedirectToAction(nameof(Index));
        }
    }
}
