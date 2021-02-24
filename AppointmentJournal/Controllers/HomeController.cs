using AppointmentJournal.AppReversedDatabase;
using AppointmentJournal.Models;
using AppointmentJournal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.Controllers
{
    public class HomeController : Controller
    {
        private IServiceProvider _serviceProvider;

        public int PageSize = 3;

        public HomeController(IServiceProvider services)
        {
            _serviceProvider = services;
        }

        public IActionResult Index() 
        {
            return View();
        }

        public ViewResult List(string category, int servicePage = 1)
        {
            var context = _serviceProvider.GetRequiredService<AppointmentJournalContext>();

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
                Services = context.Services.Where(p => category == null || p.Category.Name == category)
                    .OrderBy(p => p.CategoryId)
                    .Skip((servicePage - 1) * PageSize)
                    .Take(PageSize)
            };

            return View(servicesListViewModel);
        }
    }
}
