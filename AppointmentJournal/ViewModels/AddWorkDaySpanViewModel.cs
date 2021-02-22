using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.ViewModels
{
    public class AddWorkDaySpanViewModel
    {
        public int BeginHour { get; set; } = 8;

        public int EndHour { get; set; } = 17;

        public long ServiceId { get; set; }

        public DateTime ChosenDate { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}
