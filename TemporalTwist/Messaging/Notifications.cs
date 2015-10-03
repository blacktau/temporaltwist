namespace TemporalTwist.Messaging
{
    using System;

    public static class Notifications
    {
        public static readonly string NotifyShutdown = Guid.NewGuid().ToString();

        public static readonly string ConfirmShutdown = Guid.NewGuid().ToString();

        public static readonly string OpenWindowRequest = Guid.NewGuid().ToString();

        public static readonly string WindowClosedNotification = Guid.NewGuid().ToString(); 
        
    }
}