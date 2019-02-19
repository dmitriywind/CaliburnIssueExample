using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using IssueSample.Models;
using IssueSample.Services;
using IssueSample.ViewModels.Base;
using IssueSample.ViewModels.Nested;

namespace IssueSample.ViewModels
{
    public class MainScreenViewModel : BaseViewModel, IHandle<DeleteMessage>
    {
        private readonly IItemService _itemService;

        public MainContentViewModel MainContentViewModel { get; set; }

        public MainScreenViewModel(MainContentViewModel contentViewModel, 
                                   IEventAggregator eventAggregator,
                                   IItemService itemService)
        {
            eventAggregator.Subscribe(this);
            
            _itemService = itemService;
            MainContentViewModel = contentViewModel;
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            var items = _itemService.GetModels();
            MainContentViewModel.All.UpdateItems(items);
        }

        // Delete handler

        private bool _deleteProcessInProgress;
        private ItemViewModel _deleteItem;
        private CancellationTokenSource _deleteCancellationTokenSource;
        public bool DeleteProcessInProgress
        {
            get => _deleteProcessInProgress;
            set
            {
                if (value == _deleteProcessInProgress) return;
                _deleteProcessInProgress = value;
                NotifyOfPropertyChange();
            }
        }

        public async void Handle(DeleteMessage message)
        {
            if (IsActive)
            {
                if (_deleteItem != null && DeleteProcessInProgress && _deleteCancellationTokenSource != null)
                {
                    _deleteCancellationTokenSource.Cancel();
                    await ConfirmDelete();
                }

                DeleteProcessInProgress = true;
                
                var deleteAllViewModel = MainContentViewModel.All.Items.FirstOrDefault(gi => gi.Items.Any(i => i.Item.Id == message.Id))?.Items.FirstOrDefault(i => i.Item.Id == message.Id);
                if (deleteAllViewModel != null)
                {
                    var itemsGroup = MainContentViewModel.All.Items.First(gi => gi.Items.Any(i => i.Item.Id == message.Id));
                    int groupIndex = MainContentViewModel.All.Items.IndexOf(itemsGroup);
                    MainContentViewModel.All.Items.Remove(itemsGroup);

                    itemsGroup.Items.Remove(deleteAllViewModel);
                    if (itemsGroup.Items.Any())
                    {
                        if (groupIndex > MainContentViewModel.All.Items.Count)
                        {
                            MainContentViewModel.All.Items.Add(itemsGroup);
                        }
                        else
                        {
                            MainContentViewModel.All.Items.Insert(groupIndex, itemsGroup);
                        }
                    }

                    _deleteItem = deleteAllViewModel;
                }

                _deleteCancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = _deleteCancellationTokenSource.Token;

                await Task.Run(async () =>
                {
                    try
                    {
                        await Task.Delay(6000, cancellationToken);
                        DeleteProcessInProgress = false;
                        return await ConfirmDelete();
                    }
                    catch (TaskCanceledException)
                    {
                        // nothing bad has happened
                    }

                    return false;
                }, cancellationToken);
            }
        }
        private async Task<bool> ConfirmDelete()
        {
            if (_deleteItem != null)
            {
                var deleted = true;

                _deleteCancellationTokenSource = null;
                _deleteItem = null;

                return deleted;
            }

            return false;
        }
    }
}
