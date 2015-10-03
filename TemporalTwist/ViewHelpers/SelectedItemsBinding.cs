namespace TemporalTwist.ViewHelpers
{
    using System;
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;

    public static class SelectedItemsBinding
    {
        public static readonly DependencyProperty BindingProperty = DependencyProperty.RegisterAttached(
            "Binding", 
            typeof(IList), 
            typeof(SelectedItemsBinding), 
            new FrameworkPropertyMetadata(null, OnBindingChanged));

        public static IList GetBinding(DependencyObject dependencyObject)
        {
            return (IList)dependencyObject.GetValue(BindingProperty);
        }

        public static void SetBinding(DependencyObject dependencyObject, IList value)
        {
            var current = GetBinding(dependencyObject);
            Console.Out.WriteLine("current = {0}", current);
            if (current != value)
            {
                dependencyObject.SetValue(BindingProperty, value);
            }
        }

        private static void OnBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listBox = (ListBox)d;
            listBox.SelectionChanged -= OnListBoxSelectionChanged;

            // copy from bound list to SelectedItems
            CopyContents(GetBinding(listBox), listBox.SelectedItems);

            listBox.SelectionChanged += OnListBoxSelectionChanged;
        }

        private static void OnListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = (ListBox)e.Source;
            CopyContents(listBox.SelectedItems, GetBinding(listBox));
        }

        private static void CopyContents(IList source, IList destination)
        {
            if (source == null || destination == null)
            {
                return;
            }

            destination.Clear();
            foreach (var item in source)
            {
                destination.Add(item);
            }
        }
    }
}