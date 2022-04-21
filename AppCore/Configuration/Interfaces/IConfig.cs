namespace AppointmentJournal.AppCore;

public interface IConfig
{
    SqlOptions SqlSettings { get; }
}