using Caliburn.Micro;
using HorseBarn.lib;
using HorseBarn.lib.Cart;
using HorseBarn.lib.Horse;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HorseBarn.WPF.ViewModels
{



    public class CartViewModel : PropertyChangedBase
    {

        public delegate CartViewModel Factory(IHorseBarn horseBarn, ICart cart);

        public CartViewModel(IHorseBarn horseBarn, ICart cart, HorseViewModel.Factory horseViewModelFactory)
        {
            this.horseBarn = horseBarn;
            Cart = cart;
            this.horseViewModelFactory = horseViewModelFactory;
            cart.PropertyChanged += Cart_PropertyChanged;
            cart.CollectionChanged += Horses_CollectionChanged;
        }

        private async void Horses_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            await RebuildHorseViewModels();
        }

        private async void Cart_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ICart.NumberOfHorses))
            {
                await RebuildHorseViewModels();
            }
        }

        private async Task RebuildHorseViewModels()
        {
            var index = Math.Max(Cart.Horses.Count(), Cart.NumberOfHorses);
            var horses = Cart.Horses.GetEnumerator();
            HorseViewModels.Clear();

            for (int i = 0; i < index; i++)
            {
                var horse = horses.MoveNext();
                if (horse && i >= Cart.NumberOfHorses)
                {
                    await horseBarn.MoveHorseToPasture(horses.Current);
                }
                else if (horse)
                {
                    HorseViewModels.Add(horseViewModelFactory(horses.Current));
                }
                else
                {
                    HorseViewModels.Add(horseViewModelFactory(null));
                }
            }
        }

        public ICart Cart { get; }

        public IObservableCollection<HorseViewModel> HorseViewModels { get; } = new BindableCollection<HorseViewModel>();
        private readonly IHorseBarn horseBarn;
        private readonly HorseViewModel.Factory horseViewModelFactory;

        public async Task Plus()
        {
            Cart.NumberOfHorses++;
        }

        public async Task Minus()
        {
            Cart.NumberOfHorses--;
        }

        public async Task HandleDragDrop(object source, DragEventArgs e)
        {
            var horseViewModel = e.Data.GetData(typeof(HorseViewModel)) as HorseViewModel;
            if (horseViewModel != null && horseViewModel.Horse != null && HorseViewModels.Any(h => h.Horse == null))
            {
                await horseBarn.MoveHorseToCart(horseViewModel.Horse, Cart);
            }
        }
    }
}
