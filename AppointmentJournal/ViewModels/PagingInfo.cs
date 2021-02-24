using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.ViewModels
{
    /// <summary>
    /// Информация о страничной организации 
    /// </summary>
    public class PagingInfo
    {
        /// <summary>
        /// Общее количество элементов
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Число элементов на странице
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        /// Текущая страница
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Всего страниц
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
}
