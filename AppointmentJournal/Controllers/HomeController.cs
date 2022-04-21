using AppointmentJournal.AppDatabase;
using AppointmentJournal.Models;
using AppointmentJournal.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace AppointmentJournal.Controllers
{
    /// <summary>
    /// Контроллер для обработки запросов связанных с домашней страницей
    /// </summary>
    public class HomeController : Controller
    {
        private IServiceProvider _serviceProvider;
        private readonly UserManager<User> _userManager;

        public int PageSize = 3;

        public HomeController(IServiceProvider services, UserManager<User> userManager)
        {
            _serviceProvider = services;
            _userManager = userManager;
        }

        public IActionResult Index() 
        {
            return View();
        }

        public ViewResult List(string category, int servicePage = 1)
        {
            var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

            var services = context.Services.Where(p => category == null || p.Category.Name == category)
                    .OrderBy(p => p.CategoryId)
                    .Skip((servicePage - 1) * PageSize)
                    .Take(PageSize).AsEnumerable().Select(async x => 
                    {
                        x.Producer = await _userManager.FindByIdAsync(x.ProducerId);
                        return x;
                    })
                    .Select(x => x.Result).ToList();

            var pagingInfo = new PagingInfo()
            {
                CurrentPage = servicePage,
                ItemsPerPage = PageSize,
                TotalItems = category == null ? context.Services.Count() : context.Services.Where(e => e.Category.Name == category).Count()
            };

            var servicesListViewModel = new ServicesListViewModel()
            {
                CurrentCategory = category,
                PagingInfo = pagingInfo,
                Services = services
            };

            return View(servicesListViewModel);
        }
    }
}
