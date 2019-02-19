using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using IssueSample.Services;
using IssueSample.ViewModels;
using IssueSample.ViewModels.Nested;
using IssueSample.Views;

namespace IssueSample
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App
    {
        private WinRTContainer _container;
        private IEventAggregator _eventAggregator;

        public App()
        {
            InitializeComponent();
        }

        protected override void Configure()
        {
            //MessageBinder.SpecialValues.Add("$clickeditem", c => ((ItemClickEventArgs)c.EventArgs).ClickedItem);

            var config = new TypeMappingConfiguration
            {
                DefaultSubNamespaceForViews = "Views",
                DefaultSubNamespaceForViewModels = "ViewModels"
            };
            ViewLocator.ConfigureTypeMappings(config);
            ViewModelLocator.ConfigureTypeMappings(config);

            _container = new WinRTContainer();
            _container.RegisterWinRTServices();

            RegisterViewModels();
            RegisterServices();

            _eventAggregator = _container.GetInstance<IEventAggregator>();
        }

        private void RegisterViewModels()
        {
            _container.PerRequest<MainScreenViewModel>();
            _container.PerRequest<MainContentViewModel>()
                      .PerRequest<AllViewModel>()
                      .PerRequest<ItemViewModel>();
        }

        private void RegisterServices()
        {
            _container.Singleton<IViewModelFactoryService, ViewModelFactoryService>();
            _container.Singleton<IItemService, ItemService>();
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            _container.RegisterNavigationService(rootFrame);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            DisplayRootView(args?.Arguments);

            // It's kinda of weird having to use the event aggregator to pass 
            // a value to MainScreenViewModel, could be an argument for allowing
            // parameters or launch arguments

            if (args?.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {

            }
        }

        private void DisplayRootView(string args = null)
        {
            DisplayRootView<MainScreenView>(args);
        }

        protected override void OnSuspending(object sender, SuspendingEventArgs e)
        {

        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.Exception != null)
            {
            }
        }
    }
}
