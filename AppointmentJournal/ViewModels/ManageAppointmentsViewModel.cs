using AppointmentJournal.AppReversedDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.ViewModels
{
    /// <summary>
    /// Модель представления управления записями бронирования
    /// </summary>
    public class ManageAppointmentsViewModel
    {
        /// <summary>
        /// Список записей
        /// </summary>
        public List<Appointment> Appointments { get; set; }
    }
}
