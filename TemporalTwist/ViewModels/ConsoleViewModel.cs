namespace TemporalTwist.ViewModels
{
    using System.Windows;

    using GalaSoft.MvvmLight.CommandWpf;
    using GalaSoft.MvvmLight.Messaging;

    using TemporalTwist.Core;
    using TemporalTwist.Interfaces;
    using TemporalTwist.Messaging;

    public class ConsoleViewModel : ClosableViewModelBase
    {
        private IConsoleOutputBus consoleOutputProcessor;

        public ConsoleViewModel(IMessenger messenger, IConsoleOutputBus consoleOutputProcessor) : base(messenger)
        {
            this.Text = new ThreadSafeObservableCollection<string>();
            this.consoleOutputProcessor = consoleOutputProcessor;
        }

        public ThreadSafeObservableCollection<string> Text { get; }

        public bool IsVisible { get; set; }

        public void IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.IsVisible = (bool)e.NewValue;
            if (this.IsVisible)
            {
                this.BindConsoleListener();
            }
            else
            {
                this.UnbindConsoleListener();
            }
        }

        public void ProcessLine(string line)
        {
            this.Text.Add(line);
        }

        public override void Cleanup()
        {
            base.Cleanup();

            this.UnbindConsoleListener();

            this.consoleOutputProcessor = null;
        }

        private void UnbindConsoleListener()
        {
            if (this.consoleOutputProcessor.Listeners.Contains(this.ProcessLine))
            {
                this.consoleOutputProcessor.Listeners.Remove(this.ProcessLine);
            }
        }

        private void BindConsoleListener()
        {
            if (this.consoleOutputProcessor.Listeners.Contains(this.ProcessLine))
            {
                this.consoleOutputProcessor.Listeners.Add(this.ProcessLine);
            }
        }

        private void RegisterNotificationsInterest()
        {
            if (this.MessengerInstance == null)
            {
                return;
            }

            this.MessengerInstance.Register<ShutdownNotification>(this, this.HandleShutdownNotification);
        }

        private void UnregisterNotificationsInterests()
        {
            if (this.MessengerInstance == null)
            {
                return;
            }

            this.MessengerInstance.Unregister<ShutdownNotification>(this, this.HandleShutdownNotification);
        }

        private void HandleShutdownNotification(ShutdownNotification shutdownNotification)
        {
            this.RequestWindowClose();
        }
    }
}