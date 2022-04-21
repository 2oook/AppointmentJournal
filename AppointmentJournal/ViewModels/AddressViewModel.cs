using AppointmentJournal.AppDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.ViewModels
{
    /// <summary>
    /// Модель представления адреса
    /// </summary>
    public class AddressViewModel
    {
        /// <summary>
        /// Адрес
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// URL возврата
        /// </summary>
        public string ReturnUrl { get; set; } = "/";
    }
}
