namespace TemporalTwist.ViewHelpers
{
    using System.Windows;
    using System.Windows.Controls;

    public static class BringIntoViewWhenSelectedListBoxItemBehaviour
    {
        public static readonly DependencyProperty IsBroughtIntoViewWhenSelectedProperty =
            DependencyProperty.RegisterAttached(
                "IsBroughtIntoViewWhenSelected", 
                typeof(bool), 
                typeof(BringIntoViewWhenSelectedListBoxItemBehaviour), 
                new UIPropertyMetadata(false, OnIsBroughtIntoViewWhenSelectedChanged));

        public static bool GetIsBroughtIntoViewWhenSelected(ListBoxItem listBoxItem)
        {
            if (listBoxItem != null)
            {
                return (bool)listBoxItem.GetValue(IsBroughtIntoViewWhenSelectedProperty);
            }

            return false;
        }

        public static void SetIsBroughtIntoViewWhenSelected(ListBoxItem listBoxItem, bool value)
        {
            listBoxItem.SetValue(IsBroughtIntoViewWhenSelectedProperty, value);
        }

        private static void OnIsBroughtIntoViewWhenSelectedChanged(
            DependencyObject d, 
            DependencyPropertyChangedEventArgs e)
        {
            var item = d as ListBoxItem;
            if (item == null)
            {
                return;
            }

            if (!(e.NewValue is bool))
            {
                return;
            }

            if ((bool)e.NewValue)
            {
                item.Selected += OnListBoxItemSelected;
            }
            else
            {
                item.Selected -= OnListBoxItemSelected;
            }
        }

        private static void OnListBoxItemSelected(object sender, RoutedEventArgs e)
        {
            // ignore event bubbling
            if (!ReferenceEquals(sender, e.OriginalSource))
            {
                return;
            }

            var item = e.OriginalSource as ListBoxItem;
            item?.BringIntoView();
        }
    }
}