using AppointmentJournal.AppReversedDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.ViewModels
{
    /// <summary>
    /// Модель представления выбора даты бронирования
    /// </summary>
    public class BookAppointmentViewModel
    {
        /// <summary>
        /// Календарь рабочих дней
        /// </summary>
        public List<WorkDay[]> Dates { get; set; }

        /// <summary>
        /// Название услуги
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Идентификатор услуги
        /// </summary>
        public long ServiceId { get; set; }
    }
}
