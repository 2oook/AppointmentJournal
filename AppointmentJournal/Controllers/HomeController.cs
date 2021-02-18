using AppointmentJournal.Models;
using AppointmentJournal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.Controllers
{
    public class HomeController : Controller
    {
        private IServiceRepository repository;

        public int PageSize = 3;

        public HomeController(IServiceRepository _repository)
        {
            repository = _repository;
        }

        public IActionResult Index() 
        {
            return View();
        }

        public ViewResult List(string category, int productPage = 1)
        {
            var pagingInfo = new PagingInfo()
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = category == null ? repository.Services.Count() : repository.Services.Where(e => e.Category.Name == category).Count()
            };

            var servicesListViewModel = new ServicesListViewModel()
            {
                CurrentCategory = category,
                PagingInfo = pagingInfo,
                Services = repository.Services.Where(p => category == null || p.Category.Name == category)
                    .OrderBy(p => p.CategoryId)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize)
            };

            return View(servicesListViewModel);
        }
    }
}
