using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using AppointmentJournal.AppCore;
using AppointmentJournal.AppDatabase;
using Microsoft.EntityFrameworkCore;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AppointmentJournal;
using System;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>((hostContext, builder) =>
    {
        IConfiguration configuration = hostContext.Configuration;

        builder.RegisterType<Config>().As<IConfig>();

        var dbContextOptionsIdentity = new DbContextOptionsBuilder<AppIdentityDbContext>()
            .UseSqlServer()
            .Options;

        builder.RegisterType<AppIdentityDbContext>().As<AppIdentityDbContext>()
            .UsingConstructor(
                typeof(DbContextOptions<AppIdentityDbContext>),
                typeof(IServiceProvider), 
                typeof(IConfiguration),
                typeof(IConfig)
            )
            .WithParameter(
                new TypedParameter(
                    typeof(DbContextOptions<AppIdentityDbContext>),
                    dbContextOptionsIdentity))
            .WithParameter(
                new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IServiceProvider) && pi.Name == "serviceProvider",
                    (pi, ctx) => ctx.Resolve<IServiceProvider>()))
            .WithParameter(
                new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IConfiguration) && pi.Name == "configuration",
                    (pi, ctx) => ctx.Resolve<IConfiguration>()))
            .WithParameter(
                new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IConfig) && pi.Name == "config",
                    (pi, ctx) => ctx.Resolve<IConfig>())
            );

        var dbContextOptionsApp = new DbContextOptionsBuilder<AppointmentJournalContext>()
            .UseSqlServer()
            .Options;

        builder.RegisterType<AppointmentJournalContext>().As<AppointmentJournalContext>()
            .UsingConstructor(
                typeof(DbContextOptions<AppointmentJournalContext>),
                typeof(IConfig)
            )
            .WithParameter(
                new TypedParameter(
                    typeof(DbContextOptions<AppointmentJournalContext>),
                    dbContextOptionsApp))
            .WithParameter(
                new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IConfig) && pi.Name == "config",
                    (pi, ctx) => ctx.Resolve<IConfig>())
            );

        builder.Register(c => new UserStore<IdentityUser>(c.Resolve<AppIdentityDbContext>())).AsImplementedInterfaces().SingleInstance();
        builder.Register(c => new RoleStore<IdentityRole>(c.Resolve<AppIdentityDbContext>())).AsImplementedInterfaces().SingleInstance();   
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    })
    .Build()
    .Run();