namespace TemporalTwist.Core
{
    using System;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public abstract class BaseViewModel : ViewModelBase
    {
        protected BaseViewModel()
        {
            this.CloseCommand = new RelayCommand(this.RequestWindowClose);
        }

        public Action WindowCloseRequester { get; set; }

        public ICommand CloseCommand { get; protected set; }

        public override void Cleanup()
        {
            base.Cleanup();
            this.WindowCloseRequester = null;
        }

        protected void RequestWindowClose()
        {
            if (this.WindowCloseRequester != null)
            {
                this.WindowCloseRequester.Invoke();
                this.WindowCloseRequester = null;
            }
        }
    }
}