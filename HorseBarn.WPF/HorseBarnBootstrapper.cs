using Autofac;
using Caliburn.Micro;
using HorseBarn.lib;
using HorseBarn.lib.Horse;
using HorseBarn.WPF.ViewModels;
using Neatoo.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.WPF
{
    public class HorseBarnBootstrapper : BootstrapperBase
    {

        public HorseBarnBootstrapper()
        {
            Initialize();
        }

        //private readonly ILog _logger = LogManager.GetLog(typeof(TypedAutofacBootStrapper<>));
        private IContainer _container;

        protected IContainer Container
        {
            get { return _container; }
        }

        protected override async void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            await this.DisplayRootViewForAsync<HorseBarnViewModel>();
        }

        #region Overrides
        protected override void Configure()
        { //  configure container
            var builder = new ContainerBuilder();

            //  register the single window manager for this container
            builder.Register<IWindowManager>(c => new WindowManager()).InstancePerLifetimeScope();
            //  register the single event aggregator for this container
            builder.Register<IEventAggregator>(c => new EventAggregator()).InstancePerLifetimeScope();

            ConfigureContainer(builder);

            _container = builder.Build();


        }

        protected override object GetInstance(Type serviceType, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                if (Container.IsRegistered(serviceType))
                    return Container.Resolve(serviceType);
            }
            else
            {
                if (Container.IsRegisteredWithKey(key, serviceType))
                    return Container.ResolveKeyed(key, serviceType);
            }
            throw new Exception(string.Format("Could not locate any instances of contract {0}.", key ?? serviceType.Name));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return Container.Resolve(typeof(IEnumerable<>).MakeGenericType(serviceType)) as IEnumerable<object>;
        }

        protected override void BuildUp(object instance)
        {
            Container.InjectProperties(instance);
        }
        #endregion

        protected virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<HorseBarnViewModel>();
            builder.RegisterType<CreateHorseViewModel>();
            builder.RegisterType<CartViewModel>();
            builder.RegisterType<HorseViewModel>();

            builder.RegisterModule(new NeatooCoreModule(Portal.Local));
            builder.AutoRegisterAssemblyTypes(Assembly.GetAssembly(typeof(IHorseBarn)));
        }
    }
}
