using Caliburn.Micro;
using HorseBarn.lib;
using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
using Neatoo;
using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HorseBarn.WPF.ViewModels
{
    public class HorseBarnViewModel : Screen, IHandle<IHorseCriteria>
    {
        private readonly IWindowManager windowManager;
        private readonly IReadPortal<IHorseBarn> readWritePortalHorseBarn;
        private readonly IEventAggregator eventAggregator;
        private readonly CreateHorseViewModel createHorseViewModel;
        private readonly CartViewModel.Factory createCartViewModel;
        private readonly HorseViewModel.Factory horseViewModelFactory;
        private readonly IReadPortal<IHorseCriteria> horseCriteriaPortal;

        public HorseBarnViewModel(IWindowManager windowManager,
            IReadPortal<IHorseBarn> readWritePortalHorseBarn,
            IEventAggregator eventAggregator,
            CreateHorseViewModel createHorseViewModel,
            CartViewModel.Factory createCartViewModel,
            HorseViewModel.Factory horseViewModelFactory,
            IReadPortal<IHorseCriteria> horseCriteriaPortal)
        {
            this.windowManager = windowManager;
            this.readWritePortalHorseBarn = readWritePortalHorseBarn;
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

            HorseBarn = await readWritePortalHorseBarn.Fetch();

            // TODO - Improve fetch when the object is not found
            // how does CSLA handle this?
            if (HorseBarn.Pasture is null)
            {
                HorseBarn = await readWritePortalHorseBarn.Create();

                var horseCriteria = await horseCriteriaPortal.Create();
                horseCriteria.Name = "Secretariat";
                horseCriteria.Breed = Breed.Clydesdale;
                horseCriteria.BirthDay = new DateOnly(2010, 1, 1);

                await HorseBarn.AddNewHorse(horseCriteria);

                horseCriteria = await horseCriteriaPortal.Create();
                horseCriteria.Name = "Seattle Slew";
                horseCriteria.Breed = Breed.Clydesdale;
                horseCriteria.BirthDay = new DateOnly(2000, 1, 1);

                await HorseBarn.AddNewHorse(horseCriteria);

                horseCriteria = await horseCriteriaPortal.Create();
                horseCriteria.Name = "Speedy";
                horseCriteria.Breed = Breed.Thoroughbred;
                horseCriteria.BirthDay = new DateOnly(2010, 1, 1);

                await HorseBarn.AddNewHorse(horseCriteria);

                horseCriteria = await horseCriteriaPortal.Create();
                horseCriteria.Name = "Flash";
                horseCriteria.Breed = Breed.Mustang;
                horseCriteria.BirthDay = new DateOnly(2015, 1, 1);

                await HorseBarn.AddNewHorse(horseCriteria);


                ICart cart = await HorseBarn.AddRacingChariot();
                cart.NumberOfHorses = 2;
                cart.Name = "Racing Chariot A";

                cart = await HorseBarn.AddWagon();
                cart.NumberOfHorses = 2;
                cart.Name = "Wagon A";



                HorseBarn = await HorseBarn.SaveRetrieve<IHorseBarn>();
            }

            LoadHorseBarn();

        }

        private void LoadHorseBarn()
        {
            Horses_CollectionChanged(this, null);
            HorseBarn.Pasture.Horses.CollectionChanged += Horses_CollectionChanged;

            HorseBarn.Carts.CollectionChanged += Carts_CollectionChanged;
            Carts_CollectionChanged(null, null);

            NotifyOfPropertyChange(nameof(HorseBarn));
        }

        private void Carts_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Carts = new BindableCollection<CartViewModel>(HorseBarn.Carts.Select(c => createCartViewModel(HorseBarn, c)));
            NotifyOfPropertyChange(nameof(Carts));
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

        public IObservableCollection<CartViewModel> Carts { get; set; } = new BindableCollection<CartViewModel>();

        public async Task AddRacingChariot()
        {
            Carts.Add(createCartViewModel(HorseBarn, await HorseBarn.AddRacingChariot()));
        }

        public async Task AddWagon()
        {
            Carts.Add(createCartViewModel(HorseBarn, await HorseBarn.AddWagon()));
        }

        public void HandleDragDrop(object source, DragEventArgs e)
        {
            var horseViewModel = e.Data.GetData(typeof(HorseViewModel)) as HorseViewModel;
            if (horseViewModel != null && horseViewModel.Horse != null)
            {
                HorseBarn.MoveHorseToPasture(horseViewModel.Horse);
            }
        }

        public async void Save()
        {
            HorseBarn.Pasture.Horses.CollectionChanged -= Horses_CollectionChanged;
            HorseBarn.Carts.CollectionChanged -= Carts_CollectionChanged;
            HorseBarn = await HorseBarn.SaveRetrieve<IHorseBarn>();
            LoadHorseBarn();
        }

        //public void HandleDragOver(object source, DragEventArgs e)
        //{
        //    Debug.WriteLine("HorseBarn HandleDragOver");

        //    var horseViewModel = e.Data.GetData(typeof(HorseViewModel)) as HorseViewModel;
        //    if (horseViewModel != null && horseViewModel.Horse != null)
        //    {
        //        e.Effects = DragDropEffects.Move;
        //    }
        //    else
        //    {
        //        e.Effects = DragDropEffects.None;
        //    }
        //}
    }
}
