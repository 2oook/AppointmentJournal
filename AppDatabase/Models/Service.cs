﻿using System;
using System.Collections.Generic;

#nullable disable

namespace AppointmentJournal.AppDatabase
{
    public partial class Service
    {
        public Service()
        {
            Appointments = new HashSet<Appointment>();
            WorkDaysTimeSpans = new HashSet<WorkDaysTimeSpan>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public string ProducerId { get; set; }
        public long CategoryId { get; set; }

        public virtual ServicesCategory Category { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<WorkDaysTimeSpan> WorkDaysTimeSpans { get; set; }
    }
}
