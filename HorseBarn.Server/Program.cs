using Autofac.Extensions.DependencyInjection;
using Autofac;
using HorseBarn.lib;
using System.Reflection;
using Neatoo.Autofac;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>((container) =>
    {
        container.RegisterModule(new Neatoo.Autofac.NeatooCoreModule(Neatoo.Autofac.Portal.Local));
        container.AutoRegisterAssemblyTypes(Assembly.GetAssembly(typeof(IHorseBarn)));
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
