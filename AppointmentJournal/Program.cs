using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using AppointmentJournal;


Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
        webBuilder.UseDefaultServiceProvider(options => options.ValidateScopes = false);
    })
    .Build()
    .Run();