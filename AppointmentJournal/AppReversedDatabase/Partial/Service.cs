using AppointmentJournal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.AppReversedDatabase
{
    /// <summary>
    /// Класс представляет услугу
    /// </summary>
    public partial class Service
    {
        public User Producer { get; set; }
    }
}
