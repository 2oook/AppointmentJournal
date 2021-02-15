using System;
using System.Collections.Generic;

#nullable disable

namespace AppointmentJournal.AppReversedDatabase
{
    public partial class WorkDay
    {
        public WorkDay()
        {
            Appointments = new HashSet<Appointment>();
            WorkDaysTimeSpans = new HashSet<WorkDaysTimeSpan>();
        }

        public long Id { get; set; }
        public string ProducerId { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<WorkDaysTimeSpan> WorkDaysTimeSpans { get; set; }
    }
}
