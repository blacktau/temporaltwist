namespace TemporalTwist.Services
{
    using System.Windows;

    using GalaSoft.MvvmLight.Messaging;

    using TemporalTwist.Interfaces;
    using TemporalTwist.Interfaces.Services;
    using TemporalTwist.Messaging;

    public class ShutdownService : IShutdownService
    {
        private readonly IMessenger messenger;

        public ShutdownService(IMessenger messenger)
        {
            this.messenger = messenger;
        }

        public void RequestShutdown()
        {
            var shouldAbortShutdown = false;

            this.messenger.Send(new ConfirmShutdownNotification(shouldAbort => shouldAbortShutdown |= shouldAbort));

            if (shouldAbortShutdown)
            {
                return;
            }

            this.messenger.Send(new NotificationMessage(Notifications.NotifyShutdown));

            Application.Current.Shutdown();
        }
    }
}