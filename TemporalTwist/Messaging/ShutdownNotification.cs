namespace TemporalTwist.Messaging
{
    using GalaSoft.MvvmLight.Messaging;

    public class ShutdownNotification : NotificationMessage
    {
        public ShutdownNotification() : base(Notifications.NotifyShutdown)
        {
        }
    }
}