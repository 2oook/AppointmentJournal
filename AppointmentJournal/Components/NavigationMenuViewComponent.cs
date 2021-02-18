using AppointmentJournal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.Components
{
    // Компонент представления навигационного меню категорий
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IServiceRepository repository;

        public NavigationMenuViewComponent(IServiceRepository repo)
        {
            repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repository.Services.Select(x => x.Category.Name).Distinct().OrderBy(x => x));
        }
    }
}
