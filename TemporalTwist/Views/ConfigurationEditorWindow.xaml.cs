namespace TemporalTwist.Views
{
    using System.Windows;

    using TemporalTwist.Interfaces.Views;

    internal partial class ConfigurationEditorWindow : System.Windows.Window, IConfigurationEditorWindow
    {
        public ConfigurationEditorWindow()
        {
            this.InitializeComponent();
        }
    }
}