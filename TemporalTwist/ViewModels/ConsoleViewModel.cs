namespace TemporalTwist.ViewModels
{
    using System.Windows;

    using TemporalTwist.Core;
    using TemporalTwist.Interfaces;

    public class ConsoleViewModel : BaseViewModel
    {
        private IConsoleOutputBus consoleOutputProcessor;

        public ConsoleViewModel(IConsoleOutputBus consoleOutputProcessor)
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
    }
}