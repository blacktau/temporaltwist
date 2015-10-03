namespace TemporalTwist.ViewHelpers
{
    using System.Windows;
    using System.Windows.Controls;

    internal static class BringIntoViewWhenInsertedListBoxItemBehaviour
    {
        public static readonly DependencyProperty IsBroughtIntoViewWhenInsertedProperty =
            DependencyProperty.RegisterAttached(
                "IsBroughtIntoViewWhenInserted", 
                typeof(bool), 
                typeof(BringIntoViewWhenInsertedListBoxItemBehaviour), 
                new UIPropertyMetadata(false, OnIsBroughtIntoViewWhenInsertedChanged));

        public static bool GetIsBroughtIntoViewWhenInserted(ListBox listBox)
        {
            return (bool)listBox.GetValue(IsBroughtIntoViewWhenInsertedProperty);
        }

        public static void SetIsBroughtIntoViewWhenInserted(ListBox listBox, bool value)
        {
            listBox.SetValue(IsBroughtIntoViewWhenInsertedProperty, value);
        }

        private static void OnIsBroughtIntoViewWhenInsertedChanged(
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
                // item. += OnListBoxItemSelected;
                item.BringIntoView();
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