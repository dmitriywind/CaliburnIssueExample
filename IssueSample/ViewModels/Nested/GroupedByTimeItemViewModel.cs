using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssueSample.Models;
using IssueSample.Services;
using IssueSample.ViewModels.Base;

namespace IssueSample.ViewModels.Nested
{
    public class GroupedByTimeItemViewModel : ItemsBaseViewModel<ObservableCollection<ItemViewModel>, ItemViewModel>,
                                                       IParam<IEnumerable<ItemViewModel>>,
                                                       IGrouping<DateTime, ItemViewModel>
    {
        private readonly IViewModelFactoryService _viewModelFactoryService;

        private DateTime _dateTime;
        

        public GroupedByTimeItemViewModel(IViewModelFactoryService viewModelFactoryService)
        {
            _viewModelFactoryService = viewModelFactoryService;

            Items = new ObservableCollection<ItemViewModel>();
        }

        public async Task Fill(IGrouping<DateTime, BaseModel> itemsGroup)
        {
            Items = new ObservableCollection<ItemViewModel>();
            
            foreach (var model in itemsGroup)
            {
                Items.Add(_viewModelFactoryService.ResolveViewModel<ItemViewModel, BaseModel>(model));
            }

            Key = Items.FirstOrDefault()?.Item.DateTime.Date ?? DateTime.MinValue;
        }

        public DateTime Key
        {
            get => _dateTime;
            set
            {
                if (value.Equals(_dateTime)) return;
                _dateTime = value;
                NotifyOfPropertyChange();
            }
        }
        
        public void BindData(IEnumerable<ItemViewModel> item)
        {
            var itemViewModels = item as ItemViewModel[] ?? item.ToArray();
            foreach (var itemViewModel in itemViewModels)
            {
                Items.Add(itemViewModel);
            }

            Key = Items.FirstOrDefault()?.Item.DateTime.Date ?? DateTime.MinValue;
        }

        public IEnumerator<ItemViewModel> GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
    }
}
