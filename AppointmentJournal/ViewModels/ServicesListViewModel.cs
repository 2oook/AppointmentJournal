using AppointmentJournal.AppReversedDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.ViewModels
{
    /// <summary>
    /// Модель представления отображения списка услуг
    /// </summary>
    public class ServicesListViewModel
    {
        /// <summary>
        /// Список услуг
        /// </summary>
        public IEnumerable<Service> Services { get; set; }

        /// <summary>
        /// Объект информации о разбиении на страницы
        /// </summary>
        public PagingInfo PagingInfo { get; set; }

        /// <summary>
        /// Текущая категория
        /// </summary>
        public string CurrentCategory { get; set; }
    }
}
