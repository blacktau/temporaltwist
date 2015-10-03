using GalaSoft.MvvmLight.Messaging;

namespace TemporalTwist.Messaging
{
    using TemporalTwist.Views;

    public class OpenWindowRequest : NotificationMessage 
    {
        public OpenWindowRequest(Window viewToOpen) : base(Notifications.OpenWindowRequest)
        {
            this.ViewToOpen = viewToOpen;
        }

        public Window ViewToOpen { get; private set; }
    }
}