using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentJournal.AppReversedDatabase;

namespace AppointmentJournal.Models
{
    public interface IServiceRepository
    {
        IQueryable<Service> Services { get; }

        IQueryable<WorkDay> WorkDays { get; }
    }
}
