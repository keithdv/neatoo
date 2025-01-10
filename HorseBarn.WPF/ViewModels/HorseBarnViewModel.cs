using Caliburn.Micro;
using HorseBarn.lib;
using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.WPF.ViewModels
{
    public class HorseBarnViewModel : Screen, IHandle<IHorseCriteria>
    {
        private readonly IWindowManager windowManager;
        private readonly ISendReceivePortal<IHorseBarn> sendReceivePortalHorseBarn;
        private readonly IEventAggregator eventAggregator;
        private readonly CreateHorseViewModel createHorseViewModel;
        private readonly CartViewModel.Factory createCartViewModel;
        private readonly HorseViewModel.Factory horseViewModelFactory;
        private readonly IReceivePortal<IHorseCriteria> horseCriteriaPortal;

        public HorseBarnViewModel(IWindowManager windowManager,
            ISendReceivePortal<IHorseBarn> sendReceivePortalHorseBarn,
            IEventAggregator eventAggregator,
            CreateHorseViewModel createHorseViewModel,
            CartViewModel.Factory createCartViewModel,
            HorseViewModel.Factory horseViewModelFactory,
            IReceivePortal<IHorseCriteria> horseCriteriaPortal)
        {
            this.windowManager = windowManager;
            this.sendReceivePortalHorseBarn = sendReceivePortalHorseBarn;
            this.eventAggregator = eventAggregator;
            this.createHorseViewModel = createHorseViewModel;
            this.createCartViewModel = createCartViewModel;
            this.horseViewModelFactory = horseViewModelFactory;
            this.horseCriteriaPortal = horseCriteriaPortal;
            eventAggregator.SubscribeOnPublishedThread(this);
        }

        public IHorseBarn HorseBarn { get; private set; }
        public IObservableCollection<HorseViewModel> PastureHorses { get; private set; }

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);
            HorseBarn = await sendReceivePortalHorseBarn.Create();

            var horseCriteria = await horseCriteriaPortal.Create();
            horseCriteria.Name = "Secretariat";
            horseCriteria.Breed = Breed.Clydesdale;
            horseCriteria.BirthDay = new DateOnly(1970, 1, 1);

            await HorseBarn.AddNewHorse(horseCriteria);

            horseCriteria = await horseCriteriaPortal.Create();
            horseCriteria.Name = "Seattle Slew";
            horseCriteria.Breed = Breed.Clydesdale;
            horseCriteria.BirthDay = new DateOnly(2000, 1, 1);

            await HorseBarn.AddNewHorse(horseCriteria);

            Horses_CollectionChanged(this, null);
            HorseBarn.Pasture.Horses.CollectionChanged += Horses_CollectionChanged;
        }

        private void Horses_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            PastureHorses = new BindableCollection<HorseViewModel>(HorseBarn.Pasture.Horses.Select(h => horseViewModelFactory(h)));
            NotifyOfPropertyChange(nameof(PastureHorses));
        }

        public Task AddHorse()
        {
            return windowManager.ShowDialogAsync(createHorseViewModel);
        }

        public async Task HandleAsync(IHorseCriteria message, CancellationToken cancellationToken)
        {
            await HorseBarn.AddNewHorse(message);
        }

        public IObservableCollection<CartViewModel> Carts { get; } = new BindableCollection<CartViewModel>();

        public async Task AddRacingChariot()
        {
            Carts.Add(createCartViewModel(HorseBarn, await HorseBarn.AddRacingChariot()));
        }

        public async Task AddWagon()
        {
            Carts.Add(createCartViewModel(HorseBarn, await HorseBarn.AddWagon()));
        }
    }
}
