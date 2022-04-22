namespace AppointmentJournal.AppDatabase;

public interface IAppIdentityDbContext
{
    public Task CreateAdminAccount();
    
    public Task CreateRoles();
}