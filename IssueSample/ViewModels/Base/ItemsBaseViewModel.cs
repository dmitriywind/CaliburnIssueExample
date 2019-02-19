using System.Collections.Generic;
using Caliburn.Micro;

namespace IssueSample.ViewModels.Base
{
    public class ItemsBaseViewModel<TCollection, TItem> : BaseViewModel
        where TCollection : IEnumerable<TItem>, new()
        where TItem : PropertyChangedBase
    {
        private TCollection _items;
        private TItem _selectedItem;

        public ItemsBaseViewModel()
        {
            Items = new TCollection();
            SelectedItems = new TCollection();
        }

        public TCollection Items
        {
            get => _items;
            set
            {
                _items = value;
                NotifyOfPropertyChange();
            }
        }

        public TCollection SelectedItems { get; set; }

        public TItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }
    }
}
