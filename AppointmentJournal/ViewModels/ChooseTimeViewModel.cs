using AppointmentJournal.AppDatabase;
using AppointmentJournal.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.ViewModels
{
    /// <summary>
    /// Модель представления выбора времени бронирования
    /// </summary>
    public class ChooseTimeViewModel
    {
        /// <summary>
        /// Словарь периодов рабочего времени
        /// </summary>
        public Dictionary<WorkDaysTimeSpan, List<AppointmentTime>> AppointmentTimesList { get; set; }

        /// <summary>
        /// Идентификатор услуги
        /// </summary>
        public long ServiceId { get; set; }
    }
}
