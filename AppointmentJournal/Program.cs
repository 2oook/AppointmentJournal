using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using AppointmentJournal.AppCore;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AppointmentJournal;

Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterType<Config>().As<IConfig>();
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    })
    .Build()
    .Run();