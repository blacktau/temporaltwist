namespace TemporalTwist.Interfaces.Factories
{
    using TemporalTwist.Interfaces.Views;

    public interface IViewFactory
    {
        IConsoleWindow CreateConsoleWindow();

        IConfigurationEditorWindow CreateConfigurationEditorWindow();
    }
}