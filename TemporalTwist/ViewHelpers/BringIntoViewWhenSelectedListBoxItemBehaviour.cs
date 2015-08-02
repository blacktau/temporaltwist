// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BringIntoViewWhenSelectedListBoxItemBehaviour.cs" company="None">
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
//   contains an attached behaviour which scrolls the selected item into view.
// </remarks>
// --------------------------------------------------------------------------------------------------------------------
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