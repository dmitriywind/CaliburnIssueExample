using System.ComponentModel;
using Caliburn.Micro;
using IssueSample.Models;

namespace IssueSample.Services
{
    public interface IViewModelFactoryService
    {
        /// <summary>
        ///     Resolve viewmodel with parameter  by type
        /// </summary>
        /// <typeparam name="TViewModel">type of resolving viewmodel</typeparam>
        /// <typeparam name="TParam">type of param for resolving viewmodel</typeparam>
        /// <param name="param">parameter for resolving viewmodel</param>
        /// <returns></returns>
        TViewModel ResolveViewModel<TViewModel, TParam>(TParam param)
            where TViewModel : INotifyPropertyChanged, IParam<TParam>;

        /// <summary>
        ///     Resolve viewmodel with by type
        /// </summary>
        /// <typeparam name="TViewModel">type of resolving viewmodel</typeparam>
        /// <returns></returns>
        TViewModel ResolveViewModel<TViewModel>()
            where TViewModel : INotifyPropertyChanged;
    }

    public class ViewModelFactoryService : IViewModelFactoryService
    {
        public TViewModel ResolveViewModel<TViewModel, TParam>(TParam param)
            where TViewModel : INotifyPropertyChanged, IParam<TParam>
        {
            var vm = ResolveViewModel<TViewModel>();
            vm.BindData(param);
            return vm;
        }

        public TViewModel ResolveViewModel<TViewModel>()
            where TViewModel : INotifyPropertyChanged
        {
            var vm = IoC.Get<TViewModel>();

            return vm;
        }
    }
}
