using System;
using System.Collections.Generic;

#nullable disable

namespace AppointmentJournal.AppDatabase
{
    public partial class Address
    {
        public Address()
        {
            WorkDaysTimeSpans = new HashSet<WorkDaysTimeSpan>();
        }

        public long Id { get; set; }
        public string AddressValue { get; set; }
        public string ProducerId { get; set; }

        public virtual ICollection<WorkDaysTimeSpan> WorkDaysTimeSpans { get; set; }
    }
}
