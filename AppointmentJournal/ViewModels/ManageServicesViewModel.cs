using AppointmentJournal.AppReversedDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.ViewModels
{
    /// <summary>
    /// Модель представления списка услуг
    /// </summary>
    public class ManageServicesViewModel
    {
        /// <summary>
        /// Список услуг
        /// </summary>
        public List<Service> ServicesList { get; set; }
    }
}
