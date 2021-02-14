using AppointmentJournal.Models;
using System.ComponentModel.DataAnnotations;

namespace AppointmentJournal.ViewModels
{
    /// <summary>
    /// Модель представления страницы регистрации
    /// </summary>
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Имя и фамилия")]
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

        [Required]
        [DataType(DataType.Password)]
        [UIHint("password")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [UIHint("password")]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}
