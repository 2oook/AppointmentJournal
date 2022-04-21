using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentJournal.ViewModels
{
    /// <summary>
    /// Модель представления профиля пользователя
    /// </summary>
    public class UserProfileViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Город")]
        public string City { get; set; }

        [Required]
        [UIHint("emailaddress")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Тип пользователя")]
        public UserType UserType { get; set; }
    }
}
