// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsoleViewModel.cs" company="None">
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
//   ViewModel for the Console Window
// </remarks>
// --------------------------------------------------------------------------------------------------------------------
namespace TemporalTwist.ViewModels
{
    using System.Windows;

    using TemporalTwist.Core;
    using Interfaces;
    using System;

    public class ConsoleViewModel : BaseViewModel
    {
        private IConsoleOutputBus consoleOutputProcessor;

        public ConsoleViewModel(IConsoleOutputBus consoleOutputProcessor)
        {
            this.Text = new ThreadSafeObservableCollection<string>();
            this.consoleOutputProcessor = consoleOutputProcessor;
        }

        public ThreadSafeObservableCollection<string> Text { get; }

        public bool IsVisible { get; set; }

        public void IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.IsVisible = (bool)e.NewValue;
            if (this.IsVisible)
            {
                BindConsoleListener();
            }
            else
            {
                UnbindConsoleListener();
            }
        }

        private void UnbindConsoleListener()
        {
            if (this.consoleOutputProcessor.Listeners.Contains(this.ProcessLine))
            {
                this.consoleOutputProcessor.Listeners.Remove(this.ProcessLine);
            }
        }

        private void BindConsoleListener()
        {
            if (this.consoleOutputProcessor.Listeners.Contains(this.ProcessLine))
            {
                this.consoleOutputProcessor.Listeners.Add(this.ProcessLine);
            }
        }

        public void ProcessLine(string line)
        {
            this.Text.Add(line);
        }

        public override void Cleanup()
        {
            base.Cleanup();

            this.UnbindConsoleListener();

            this.consoleOutputProcessor = null;
        }
    }
}