using System;
using System.Collections.Generic;

#nullable disable

namespace AppointmentJournal.AppReversedDatabase
{
    public partial class ServicesCategory
    {
        public ServicesCategory()
        {
            Services = new HashSet<Service>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Service> Services { get; set; }
    }
}
