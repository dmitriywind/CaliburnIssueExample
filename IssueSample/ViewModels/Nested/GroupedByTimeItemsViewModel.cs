using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using IssueSample.Models;
using IssueSample.Services;
using IssueSample.ViewModels.Base;

namespace IssueSample.ViewModels.Nested
{
    public class GroupedByTimeItemsViewModel : ItemsBaseViewModel<ObservableCollection<GroupedByTimeItemViewModel>, GroupedByTimeItemViewModel>
    {
        private readonly IViewModelFactoryService _viewModelFactoryService;

        private ItemViewModel _selection;

        public GroupedByTimeItemsViewModel(IViewModelFactoryService viewModelFactoryService)
        {
            _viewModelFactoryService = viewModelFactoryService;
        }

        public async void UpdateItems(List<BaseModel> items)
        {
            Items.Clear();

            var result = new List<GroupedByTimeItemViewModel>();

            foreach (var itemsGroup in items.GroupBy(m => m.DateTime.Date))
            {
                var itemsGroupedByTimeItemViewModel = new GroupedByTimeItemViewModel(_viewModelFactoryService);
                await itemsGroupedByTimeItemViewModel.Fill(itemsGroup);
                result.Add(itemsGroupedByTimeItemViewModel);
            }

            Items = new ObservableCollection<GroupedByTimeItemViewModel>(result);
        }
        
        public ItemViewModel Selection
        {
            get => _selection;
            set
            {
                _selection = value;
                NotifyOfPropertyChange(() => Selection);

                if (_selection != null)
                {
                    var groupKey = _selection.Item.DateTime.Date;
                    foreach (var item in Items)
                    {
                        var isItRightGroup = item.Key == groupKey;
                        foreach (var itemViewModel in item.Items)
                        {
                            itemViewModel.IsSelected = itemViewModel.Item.Id == _selection.Item?.Id && isItRightGroup;
                        }
                    }
                }
            }
        }
    }
}
