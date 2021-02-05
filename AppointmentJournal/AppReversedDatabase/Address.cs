using System;
using System.Collections.Generic;

#nullable disable

namespace AppointmentJournal.AppReversedDatabase
{
    public partial class Address
    {
        public Address()
        {
            Appointments = new HashSet<Appointment>();
        }

        public long Id { get; set; }
        public string AddressValue { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
