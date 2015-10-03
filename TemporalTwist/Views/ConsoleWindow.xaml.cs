namespace TemporalTwist.Views
{
    using System.Windows;

    using TemporalTwist.Interfaces.Views;

    internal partial class ConsoleWindow : System.Windows.Window, IConsoleWindow
    {
        public ConsoleWindow()
        {
            this.InitializeComponent();
        }
    }
}