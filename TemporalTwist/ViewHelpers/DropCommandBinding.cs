// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DropCommandBinding.cs" company="None">
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
// adds a depenency property which allows a command to handle Drag and Drop behaviour.
// </remarks>
// --------------------------------------------------------------------------------------------------------------------
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