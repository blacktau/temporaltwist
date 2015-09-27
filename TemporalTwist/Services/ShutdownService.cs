namespace TemporalTwist.Services
{
    using System.Windows;

    using GalaSoft.MvvmLight.Messaging;

    using TemporalTwist.Messaging;

    public class ShutdownService
    {
        public static void RequestShutdown()
        {
            var shouldAbortShutdown = false;

            Messenger.Default.Send(
                new NotificationMessageAction<bool>(
                    Notifications.ConfirmShutdown, 
                    shouldAbort => shouldAbortShutdown |= shouldAbort));

            if (shouldAbortShutdown)
            {
                return;
            }

            Messenger.Default.Send(new NotificationMessage(Notifications.NotifyShutdown));

            Application.Current.Shutdown();
        }
    }
}