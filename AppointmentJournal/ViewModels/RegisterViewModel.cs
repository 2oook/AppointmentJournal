using System.ComponentModel.DataAnnotations;

namespace AppointmentJournal.ViewModels
{
    /// <summary>
    /// Модель представления страницы регистрации
    /// </summary>
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Логин не задан")]
        [Display(Name = "Логин")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Город не задан")]
        [Display(Name = "Город")]
        public string City { get; set; }

        [Required(ErrorMessage = "Электронный адрес не задан")]
        [UIHint("emailaddress")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Номер телефона не задан")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Тип пользователя не задан")]
        [Display(Name = "Тип пользователя")]
        public UserType UserType { get; set; }

        [Required(ErrorMessage = "Пароль не задан")]
        [DataType(DataType.Password)]
        [UIHint("password")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтверждение пароля не задано")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [UIHint("password")]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}
