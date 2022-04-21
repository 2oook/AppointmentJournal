using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.Models
{
    /// <summary>
    /// User
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }
    }

    /// <summary>
    /// Type of app user
    /// </summary>
    public enum UserType
    {
        None,
        [Display(Name ="Service consumer")]
        Consumer,
        [Display(Name = "Service producer")]
        Producer
    }
}
