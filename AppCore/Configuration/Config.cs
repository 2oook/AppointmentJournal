using Microsoft.Extensions.Options;

namespace AppointmentJournal.AppCore;

public class Config: IConfig
{
    public Config(IOptions<SqlOptions> sqlOptions)
    {
        this.SqlSettings = sqlOptions.Value;
    }

    public SqlOptions SqlSettings { get; set; }
}