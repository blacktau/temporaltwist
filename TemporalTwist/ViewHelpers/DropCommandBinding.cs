namespace TemporalTwist.ViewHelpers
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    public static class DropCommandBinding
    {
        public static readonly DependencyProperty DropCommandProperty =
            DependencyProperty.RegisterAttached(
                "DropCommand", 
                typeof(ICommand), 
                typeof(DropCommandBinding), 
                new FrameworkPropertyMetadata(null, OnBindingChanged));

        public static ICommand GetDropCommand(DependencyObject dependencyObject)
        {
            return (ICommand)dependencyObject.GetValue(DropCommandProperty);
        }

        public static void SetDropCommand(DependencyObject dependencyObject, ICommand command)
        {
            var current = GetDropCommand(dependencyObject);
            Console.Out.WriteLine("current = {0}", current);
            if (current != command)
            {
                var source = dependencyObject as UIElement;
                if (source != null && command != null)
                {
                    source.Drop += (sender, args) =>
                        {
                            var data = args.Data.GetData("FileDrop");
                            command.Execute(data);
                        };

                    dependencyObject.SetValue(DropCommandProperty, command);
                }
            }
        }

        private static void OnBindingChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var source = dependencyObject as UIElement;
            var command = GetDropCommand(dependencyObject);
            if (e.NewValue != command || e.OldValue == null)
            {
                if (source != null && command != null)
                {
                    source.Drop += (sender, args) =>
                        {
                            var data = args.Data.GetData(DataFormats.FileDrop, true);
                            command.Execute(data);
                        };
                }
            }
        }
    }
}