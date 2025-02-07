using Microsoft.Extensions.DependencyInjection;
using HorseBarn.lib;
using System.Reflection;
using HorseBarn.Dal.Ef;
using Neatoo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddNeatooServices(PortalServer.Local);

builder.Services.AutoRegisterAssemblyTypes(Assembly.GetExecutingAssembly());

builder.Services.AutoRegisterAssemblyTypes(Assembly.GetAssembly(typeof(IHorseBarn)));

builder.Services.AddScoped<IHorseBarnContext, HorseBarnContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
