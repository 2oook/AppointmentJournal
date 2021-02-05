using System;
using System.Collections.Generic;

#nullable disable

namespace AppointmentJournal.AppReversedDatabase
{
    public partial class WorkDaysTimeSpan
    {
        public long Id { get; set; }
        public long WorkDayId { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

        public virtual WorkDay WorkDay { get; set; }
    }
}
