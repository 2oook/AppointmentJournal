using AppointmentJournal.AppReversedDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.ViewModels
{
    public class BookAppointmentViewModel
    {
        public List<WorkDay[]> Dates { get; set; }

        public int ServiceId { get; set; }
    }
}
