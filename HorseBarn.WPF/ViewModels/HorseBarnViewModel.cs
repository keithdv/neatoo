using Caliburn.Micro;
using HorseBarn.lib;
using Neatoo.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseBarn.WPF.ViewModels
{
    public class HorseBarnViewModel : Screen
    {
        private readonly ISendReceivePortal<IHorseBarn> sendReceivePortalHorseBarn;

        public HorseBarnViewModel(ISendReceivePortal<IHorseBarn> sendReceivePortalHorseBarn)
        {
            this.sendReceivePortalHorseBarn = sendReceivePortalHorseBarn;
        }

        public IHorseBarn HorseBarn { get; private set; }

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);
            HorseBarn = await sendReceivePortalHorseBarn.Create();            
        }

        public async Task AddHorse()
        {
            var horse = await HorseBarn.AddNewHorse(lib.Horse.Breed.Thoroughbred);

        }
    }
}
