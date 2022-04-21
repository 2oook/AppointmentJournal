using Microsoft.AspNetCore.Identity;

namespace AppointmentJournal.AppDatabase;

/// <summary>
/// User
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    /// City
    /// </summary>
    public string? City { get; set; }
}