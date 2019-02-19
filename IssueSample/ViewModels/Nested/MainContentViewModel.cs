using IssueSample.ViewModels.Base;

namespace IssueSample.ViewModels.Nested
{
    public class MainContentViewModel : BaseViewModel
    {
        public AllViewModel All { get; set; }

        public MainContentViewModel(AllViewModel allViewModel)
        {
            All = allViewModel;
        }
    }
}
