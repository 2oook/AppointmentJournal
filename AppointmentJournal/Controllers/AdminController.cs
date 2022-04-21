using AppointmentJournal.AppDatabase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentJournal.Controllers;

/// <summary>
/// Admin's controller
/// </summary>
[Authorize(Roles = DatabaseConstants.AdminsRole)]
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