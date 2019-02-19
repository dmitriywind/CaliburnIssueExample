using Caliburn.Micro;
using IssueSample.Models;

namespace IssueSample.ViewModels.Nested
{
    public class ItemViewModel : PropertyChangedBase, IParam<BaseModel>
    {
        private readonly IEventAggregator _eventAggregator;

        private bool _isSelected;

        public BaseModel Item { get; private set; }

        public ItemViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void BindData(BaseModel item)
        {
            Item = item;
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                NotifyOfPropertyChange(() => IsSelected);
            }
        }

        public void Delete()
        {
            _eventAggregator.PublishOnUIThread(new DeleteMessage
            {
                Id = Item.Id
            });
        }

        public void Edit()
        {
            _eventAggregator.PublishOnUIThread(new EditMessage
            {
                Id = Item.Id,
            });
        }
    }
}
