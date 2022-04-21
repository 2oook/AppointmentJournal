using AppointmentJournal.AppDatabase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.Controllers
{
    /// <summary>
    /// Контроллер для обработки запросов связанных с администрированием приложения
    /// </summary>
    [Authorize(Roles = "Admins")]
    public class AdminController : Controller
    {      
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Метод для наполнения БД тестовыми данными
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SeedDatabaseWithFakeData()
        {          
            var fakeDataProvider = new FakeDataForDBProvider(HttpContext.RequestServices);

            fakeDataProvider.PopulateDBWithFakeData(HttpContext.RequestServices);

            return RedirectToAction(nameof(Index));
        }
    }
}
