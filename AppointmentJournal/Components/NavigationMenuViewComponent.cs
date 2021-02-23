using AppointmentJournal.AppReversedDatabase;
using AppointmentJournal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.Components
{
    // Компонент представления навигационного меню категорий
    public class NavigationMenuViewComponent : ViewComponent
    {
        private AppointmentJournalDbContext context;

        public NavigationMenuViewComponent(IServiceProvider _serviceProvider)
        {
            context = _serviceProvider.GetRequiredService<AppointmentJournalDbContext>();
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(context.Services.Select(x => x.Category.Name).Distinct().OrderBy(x => x));
        }
    }
}
