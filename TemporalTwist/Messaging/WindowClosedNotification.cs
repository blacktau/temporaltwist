namespace TemporalTwist.Messaging
{
    using GalaSoft.MvvmLight.Messaging;

    using TemporalTwist.Views;

    public class WindowClosedNotification : NotificationMessage
    {
        public WindowClosedNotification(Window viewClosed) : base(Notifications.WindowClosedNotification)
        {
            this.ViewClosed = viewClosed;
        }

        public Window ViewClosed { get; private set; }
    }
}