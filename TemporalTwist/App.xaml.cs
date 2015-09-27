namespace TemporalTwist
{
    using System;
    using System.Windows;

    internal partial class App : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
        }

        protected override void OnExit(ExitEventArgs e)
        {
        }
    }
}