namespace TemporalTwist.Core
{
    using System;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    public abstract class ClosableViewModelBase : ViewModelBase
    {
        private bool closeTrigger;

        protected ClosableViewModelBase(IMessenger messenger) : base(messenger)
        {
            this.CloseCommand = new RelayCommand(this.RequestWindowClose);
        }

        public ICommand CloseCommand { get; protected set; }

        public bool CloseTrigger
        {
            get
            {
                return this.closeTrigger;
            }

            private set
            {
                if (this.closeTrigger == value)
                {
                    return;
                }

                this.closeTrigger = value;
                this.RaisePropertyChanged();
            }
        }

        protected void RequestWindowClose()
        {
            this.CloseTrigger = true;
        }
    }
}