using AppointmentJournal.AppReversedDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.ViewModels
{
    public class AddressViewModel
    {
        public Address Address { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}
