﻿using System;
using System.Collections.Generic;

#nullable disable

namespace AppointmentJournal.AppReversedDatabase
{
    public partial class Appointment
    {
        public long Id { get; set; }
        public long ServiceId { get; set; }
        public DateTime Time { get; set; }
        public long WorkDayTimeSpanId { get; set; }

        public virtual Service Service { get; set; }
        public virtual WorkDaysTimeSpan WorkDayTimeSpan { get; set; }
    }
}
