using System;
using System.Collections.Generic;

#nullable disable

namespace AppointmentJournal.AppReversedDatabase
{
    public partial class Appointment
    {
        public long Id { get; set; }
        public long ServiceId { get; set; }
        public DateTime Time { get; set; }
        public long WorkDayId { get; set; }
        public long AddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual Service Service { get; set; }
        public virtual WorkDay WorkDay { get; set; }
    }
}
