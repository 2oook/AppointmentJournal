using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.Models
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// Город
        /// </summary>
        public string City { get; set; }
    }

    /// <summary>
    /// Тип пользователя приложения
    /// </summary>
    public enum UserType 
    {
        [Display(Name ="Потребитель услуг")]
        Consumer,
        [Display(Name = "Производитель услуг")]
        Producer
    }
}
