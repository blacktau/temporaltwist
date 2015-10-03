namespace TemporalTwist.Messaging
{
    using System;
    using GalaSoft.MvvmLight.Messaging;

    public class ConfirmShutdownNotification : NotificationMessageAction<bool>
    {
        public ConfirmShutdownNotification(Action<bool> callback) : base(Notifications.ConfirmShutdown, callback)
        {
        }
    }
}