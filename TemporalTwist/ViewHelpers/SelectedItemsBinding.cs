// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectedItemsBinding.cs" company="None">
//   Copyright (c) 2009, Sean Garrett
//   All rights reserved.
//   Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
//   following conditions are met:
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and 
//      the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//      the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * The names of the contributors may not be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
//   INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
//   DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
//   SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
//   SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//   WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE 
//   USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// <remarks>
//   Dependency Property which allows for DataBinding to the MultiSelect Capability of a ListBox.
// </remarks>
// --------------------------------------------------------------------------------------------------------------------
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