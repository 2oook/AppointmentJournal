using System;
using System.Collections.Generic;

#nullable disable

namespace AppointmentJournal.AppReversedDatabase
{
    public partial class WorkDaysTimeSpan
    {
        public WorkDaysTimeSpan()
        {
            Appointments = new HashSet<Appointment>();
        }

        public long Id { get; set; }
        public long WorkDayId { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public long ServiceId { get; set; }
        public long? AddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual Service Service { get; set; }
        public virtual WorkDay WorkDay { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
