using HorseBarn.lib;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Neatoo;
using System.Reflection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddNeatooServices(DataMapperHost.Remote);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AutoRegisterAssemblyTypes(Assembly.GetAssembly(typeof(IHorseBarn)));

await builder.Build().RunAsync();
