namespace TemporalTwist.ViewModels
{
    using CommonServiceLocator.NinjectAdapter.Unofficial;
    using Microsoft.Practices.ServiceLocation;
    using Ninject;
    using GalaSoft.MvvmLight.Messaging;

    using TemporalTwist.Services;

    public class ViewModelLocator
    {
        private StandardKernel kernel;

        private WindowService windowService;

        #region Constructors and Destructors

        public ViewModelLocator()
        {
            this.InitialiseKernel();
        }

        private void InitialiseKernel()
        {
            this.kernel = new StandardKernel();
            this.kernel.Load<Module>();
            this.kernel.Bind<IMessenger>().To<Messenger>().InSingletonScope();

            ServiceLocator.SetLocatorProvider(() => new NinjectServiceLocator(this.kernel));

            this.windowService = ServiceLocator.Current.GetInstance<WindowService>();
        }

        #endregion

        #region Public Properties

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public ConsoleViewModel ConsoleViewModel => ServiceLocator.Current.GetInstance<ConsoleViewModel>();

        public ConfigurationViewModel ConfigurationViewModel => ServiceLocator.Current.GetInstance<ConfigurationViewModel>();

        public UpdateNotificationViewModel UpdateNotificationViewModel => ServiceLocator.Current.GetInstance<UpdateNotificationViewModel>();

        #endregion
    }
}