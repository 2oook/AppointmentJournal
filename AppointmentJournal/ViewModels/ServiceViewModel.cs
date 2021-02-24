using AppointmentJournal.AppReversedDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.ViewModels
{
    /// <summary>
    /// Модель представления для услуги
    /// </summary>
    public class ServiceViewModel
    {
        /// <summary>
        /// Услуга
        /// </summary>
        public Service Service { get; set; }

        /// <summary>
        /// Категория
        /// </summary>
        public ServicesCategory Category { get; set; } = new ServicesCategory();
    }
}
