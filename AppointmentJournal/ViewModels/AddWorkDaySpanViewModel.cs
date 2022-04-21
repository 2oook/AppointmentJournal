using AppointmentJournal.AppDatabase;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.ViewModels
{
    /// <summary>
    /// Модель представления добавления периода рабочего времени
    /// </summary>
    public class AddWorkDaySpanViewModel
    {
        /// <summary>
        /// Час начала периода
        /// </summary>
        public int BeginHour { get; set; } = 8;

        /// <summary>
        /// Час конца периода
        /// </summary>
        public int EndHour { get; set; } = 17;

        /// <summary>
        /// Идентификатор услуги
        /// </summary>
        public long ServiceId { get; set; }

        /// <summary>
        /// Выбранная дата
        /// </summary>
        public DateTime ChosenDate { get; set; }

        /// <summary>
        /// Идентификатор адреса
        /// </summary>
        public long AddressId { get; set; }

        /// <summary>
        /// Список адресов для выпадающего списка
        /// </summary>
        public List<SelectListItem> Addresses { get; set; }

        /// <summary>
        /// URL возврата
        /// </summary>
        public string ReturnUrl { get; set; } = "/";
    }
}
