using Microsoft.Extensions.DependencyInjection;
using Caliburn.Micro;
using HorseBarn.lib;
using HorseBarn.WPF.ViewModels;
using System.Reflection;
using System.Net.Http;
using Neatoo;

namespace HorseBarn.WPF;

public class HorseBarnBootstrapper : BootstrapperBase
{

    public HorseBarnBootstrapper()
    {
        Initialize();
    }

    //private readonly ILog _logger = LogManager.GetLog(typeof(TypedAutofacBootStrapper<>));
    private IServiceProvider _serviceProvider;
    private IHttpClientFactory httpClientFactory;
    private IHttpClientFactory _httpClientFactory;

    protected IServiceProvider ServiceProvider
    {
        get { return _serviceProvider; }
    }

    protected override async void OnStartup(object sender, System.Windows.StartupEventArgs e)
    {
        await this.DisplayRootViewForAsync<HorseBarnViewModel>();
    }

    protected override void OnExit(object sender, EventArgs e)
    {
        base.OnExit(sender, e);
    }

    #region Overrides
    protected override void Configure()
    { //  configure container
        var services = new ServiceCollection();

        // Register the single window manager for this container
        services.AddSingleton<IWindowManager, WindowManager>();
        // Register the single event aggregator for this container
        services.AddSingleton<IEventAggregator, EventAggregator>();

        ConfigureContainer(services);

        _serviceProvider = services.BuildServiceProvider();


    }

    protected override object GetInstance(Type serviceType, string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return _serviceProvider.GetRequiredService(serviceType);
        }
        else
        {
            return _serviceProvider.GetKeyedServices(serviceType, key);
        }
    }

    protected override IEnumerable<object> GetAllInstances(Type serviceType)
    {
        return _serviceProvider.GetRequiredService(typeof(IEnumerable<>).MakeGenericType(serviceType)) as IEnumerable<object>;
    }

    protected override void BuildUp(object instance)
    {
        var serviceProvider = _serviceProvider.CreateScope().ServiceProvider;
        foreach (var property in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.CanWrite && property.GetValue(instance) == null)
            {
                var service = serviceProvider.GetService(property.PropertyType);
                if (service != null)
                {
                    property.SetValue(instance, service);
                }
            }
        }
    }

    #endregion

    protected virtual void ConfigureContainer(ServiceCollection services)
    {

        services.AddHttpClient();

        _httpClientFactory = services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>();

        services.AddSingleton(_ =>
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:5037/");
            return client;
        });

        services.AddTransient<HorseBarnViewModel>();
        services.AddTransient<CreateHorseViewModel>();
        services.AddTransient<CartViewModel>();
        services.AddTransient<HorseViewModel>();

        services.AddTransient<CartViewModel.Factory>(services =>
        {
            return (horseBarn, cart) => new CartViewModel(horseBarn, cart, services.GetRequiredService<HorseViewModel.Factory>());
        });

        services.AddTransient<HorseViewModel.Factory>(services =>
        {
            return (horse) => new HorseViewModel(horse);
        });

        services.AddNeatooServices(DataMapperHost.Remote);

        services.AutoRegisterAssemblyTypes(Assembly.GetAssembly(typeof(IHorseBarn)));
    }
}
