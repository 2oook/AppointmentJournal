using AppointmentJournal.AppReversedDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.ViewModels
{
    /// <summary>
    /// Модель представления управления адресами
    /// </summary>
    public class ManageAddressesViewModel
    {
        /// <summary>
        /// Список адресов
        /// </summary>
        public List<Address> AddressList { get; set; }
    }
}
