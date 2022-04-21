using System.ComponentModel.DataAnnotations;

namespace AppointmentJournal;

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