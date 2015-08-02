namespace TemporalTwist.ViewModels
{
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
        }

        #endregion

        #region Public Properties

        public MainViewModel Main => this.kernel.Get<MainViewModel>();
        
        #endregion
    }
}