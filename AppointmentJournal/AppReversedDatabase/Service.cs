using System;
using System.Collections.Generic;

#nullable disable

namespace AppointmentJournal.AppReversedDatabase
{
    public partial class Service
    {
        public Service()
        {
            Appointments = new HashSet<Appointment>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public long ProducerId { get; set; }
        public long ServiceId { get; set; }

        public virtual ServicesCategory ServiceNavigation { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
