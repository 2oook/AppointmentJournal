using System.ComponentModel.DataAnnotations;

namespace AppointmentJournal.ViewModels
{
    /// <summary>
    /// Модель представления страницы входа
    /// </summary>
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [UIHint("password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}
