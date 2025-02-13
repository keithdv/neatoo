using Caliburn.Micro;
using HorseBarn.lib.Horse;
using Neatoo.Portal;
using System.Windows;

namespace HorseBarn.WPF.ViewModels;

public class CreateHorseViewModel : Caliburn.Micro.Screen
{
    private readonly IEventAggregator eventAggregator;

    public CreateHorseViewModel(IReadPortal<IHorseCriteria> horseCriteriaPortal,
        IEventAggregator eventAggregator)
    {
        HorseCriteriaPortal = horseCriteriaPortal;
        this.eventAggregator = eventAggregator;
    }

    public IReadPortal<IHorseCriteria> HorseCriteriaPortal { get; }

    public IHorseCriteria HorseCriteria { get; private set; }

    public List<Breed> Breeds => Enum.GetValues<Breed>().ToList();

    protected override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        HorseCriteria = await HorseCriteriaPortal.Create();
        HorseCriteria.Breed = Breed.Thoroughbred;
        NotifyOfPropertyChange(nameof(HorseCriteria));
        await base.OnActivateAsync(cancellationToken);
    }

    public async Task create()
    {
        if(!HorseCriteria.IsValid)
        {
            MessageBox.Show("Invalid Horse Criteria");
            return;
        }
        await eventAggregator.PublishOnUIThreadAsync(HorseCriteria);
        await this.TryCloseAsync();
    }

    public async Task cancel()
    {
        this.HorseCriteria = null;
        await this.TryCloseAsync();
    }
}
