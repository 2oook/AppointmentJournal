using AppointmentJournal.AppReversedDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.ViewModels
{
    public class ServiceViewModel
    {
        public Service Service { get; set; }

        public ServicesCategory Category { get; set; } = new ServicesCategory();
    }
}
