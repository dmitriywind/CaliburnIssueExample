using Caliburn.Micro;
using IssueSample.Services;

namespace IssueSample.ViewModels.Nested
{
    public class AllViewModel : GroupedByTimeItemsViewModel
    {
        public AllViewModel(IViewModelFactoryService viewModelFactoryService, IEventAggregator eventAggregator) : base(viewModelFactoryService)
        {
            eventAggregator.Subscribe(this);
        }
    }
}
