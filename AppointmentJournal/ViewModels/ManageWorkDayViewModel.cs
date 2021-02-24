using AppointmentJournal.AppReversedDatabase;
using AppointmentJournal.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.ViewModels
{
    /// <summary>
    /// Модель представления управления рабочим днём
    /// </summary>
    public class ManageWorkDayViewModel
    {
        /// <summary>
        /// Словарь периодов рабочего времени
        /// </summary>
        public Dictionary<WorkDaysTimeSpan, List<AppointmentTime>> AppointmentTimesList { get; set; }

        /// <summary>
        /// Идентификатор услуги
        /// </summary>
        public long ServiceId { get; set; }

        /// <summary>
        /// Рабочий день
        /// </summary>
        public WorkDay WorkDay { get; set; }

        /// <summary>
        /// Выбранная дата
        /// </summary>
        public DateTime ChosenDate { get; set; }
    }
}
