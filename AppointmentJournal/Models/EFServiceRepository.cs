using AppointmentJournal.AppReversedDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.Models
{
    public class EFServiceRepository : IServiceRepository
    {
        private AppointmentJournalContext appointmentJournalContext;

        public EFServiceRepository(AppointmentJournalContext context)
        {
            appointmentJournalContext = context;
        }

        public IQueryable<Service> Services => appointmentJournalContext.Services;
    }
}
