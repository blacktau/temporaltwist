namespace TemporalTwist.Services
{
    using System;

    using GalaSoft.MvvmLight.Messaging;

    using TemporalTwist.Interfaces.Factories;
    using TemporalTwist.Interfaces.Services;
    using TemporalTwist.Interfaces.Views;
    using TemporalTwist.Messaging;
    using TemporalTwist.Views;

    public class WindowService : IDisposable, IWindowService
    {
        private readonly IMessenger messenger;

        private readonly IViewFactory viewFactory;

        private IConsoleWindow consoleWindow;

        public WindowService(IMessenger messenger, IViewFactory viewFactory)
        {
            this.messenger = messenger;
            this.viewFactory = viewFactory;
            this.RegisterNotificastionInterests();
        }

        public void Dispose()
        {
        }

        private void RegisterNotificastionInterests()
        {
            this.messenger.Register<OpenWindowRequest>(this, this.HandleOpenWindowRequest);
        }

        private void HandleOpenWindowRequest(OpenWindowRequest request)
        {
            switch (request.ViewToOpen)
            {
                case Window.Console:

                    this.OpenConsoleWindow();
                    break;

                case Window.ConfigurationEditor:

                    this.OpenConfigurationWindow();
                    break;

            }
        }

        private void OpenConfigurationWindow()
        {
            var formatView = this.viewFactory.CreateConfigurationEditorWindow();
            formatView.ShowDialog();
        }

        private void OpenConsoleWindow()
        {
            if (this.consoleWindow == null)
            {
                this.consoleWindow = this.viewFactory.CreateConsoleWindow();
            }

            this.consoleWindow.Show();
            this.consoleWindow.Focus();
        }
    }
}