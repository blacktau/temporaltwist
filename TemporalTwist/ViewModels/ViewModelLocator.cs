namespace TemporalTwist.ViewModels
{
    using CommonServiceLocator.NinjectAdapter.Unofficial;
    using Microsoft.Practices.ServiceLocation;
    using Ninject;

    public class ViewModelLocator
    {
        private StandardKernel kernel;

        #region Constructors and Destructors

        public ViewModelLocator()
        {
            this.InitialiseKernel();
        }

        private void InitialiseKernel()
        {
            this.kernel = new StandardKernel();
            this.kernel.Load<Module>();
            ServiceLocator.SetLocatorProvider(() => new NinjectServiceLocator(this.kernel));
        }

        #endregion

        #region Public Properties

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        
        #endregion
    }
}