namespace TemporalTwist.ViewHelpers
{
    using System;
    using System.Windows;
    using System.Windows.Interactivity;

    public class CloseWindowBehaviour : Behavior<Window>
    {
        public static readonly DependencyProperty CloseTriggerProperty = DependencyProperty.Register("CloseTrigger", typeof(bool), typeof(CloseWindowBehaviour), new PropertyMetadata(false, OnCloseTriggerChanged));

        public bool CloseTrigger
        {
            get
            {
                return (bool)this.GetValue(CloseTriggerProperty);
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private static void OnCloseTriggerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behaviour = d as CloseWindowBehaviour;

            if (behaviour == null)
            {
                return;
            }

            behaviour.TryCloseAssociatedObject();
        }

        private void TryCloseAssociatedObject()
        {
            if (!this.CloseTrigger)
            {
                return;
            }

            this.AssociatedObject.Close();
        }
    }
}