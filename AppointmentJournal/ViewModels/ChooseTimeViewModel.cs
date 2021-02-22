﻿using AppointmentJournal.AppReversedDatabase;
using AppointmentJournal.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.ViewModels
{
    public class ChooseTimeViewModel
    {
        public Dictionary<WorkDaysTimeSpan, List<AppointmentTime>> AppointmentTimesList { get; set; }

        public long ServiceId { get; set; }
    }
}
