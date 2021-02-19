using System;
using System.Collections.Generic;

#nullable disable

namespace AppointmentJournal.AppReversedDatabase
{
    public partial class WorkDay
    {
        public WorkDay()
        {
            WorkDaysTimeSpans = new HashSet<WorkDaysTimeSpan>();
        }

        public long Id { get; set; }
        public string ProducerId { get; set; }
        public DateTime Date { get; set; }
        public bool IsEnabled { get; set; }

        public virtual ICollection<WorkDaysTimeSpan> WorkDaysTimeSpans { get; set; }
    }
}
