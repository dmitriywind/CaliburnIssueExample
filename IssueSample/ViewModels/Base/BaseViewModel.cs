using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace IssueSample.ViewModels.Base
{
    public class BaseViewModel : Screen
    {
        private bool _isBusy;

        public Action<bool> OnBusyChangedAction;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (value.Equals(_isBusy)) return;
                _isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
                OnBusyChangedAction?.Invoke(_isBusy);
            }
        }
    }
}
