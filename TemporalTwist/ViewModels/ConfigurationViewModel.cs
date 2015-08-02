// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationViewModel.cs" company="None">
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
// --------------------------------------------------------------------------------------------------------------------
namespace TemporalTwist.ViewModels
{
    using System;
    using System.Reflection;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    using TemporalTwist.Configuration;
    using TemporalTwist.Core;
    using TemporalTwist.Services;

    public class ConfigurationViewModel : BaseViewModel
    {
        private readonly ConfigurationService configurationService;

        private bool isChecking;

        private readonly Configuration configuration;

        public ConfigurationViewModel(ConfigurationService configurationService)
        {
            this.configurationService = configurationService;
            this.FormatEditorViewModel = new FormatEditorViewModel();
            this.UpdateCheckCommand = new RelayCommand(this.CheckForUpdates, () => !this.IsChecking);

            this.configuration = this.configurationService.GetConfiguration();
        }

        public FormatEditorViewModel FormatEditorViewModel { get; private set; }

        public ICommand UpdateCheckCommand { get; private set; }

        public bool IsChecking
        {
            get
            {
                return this.isChecking;
            }

            private set
            {
                if (value == this.isChecking)
                {
                    return;
                }

                this.isChecking = value;
                this.RaisePropertyChanged(nameof(this.IsChecking));
            }
        }

        public bool UpdateCheckAtStartup
        {
            get
            {
                return this.configuration.CheckForUpdatesAtStart;
            }

            set
            {
                var currentValue = this.configuration.CheckForUpdatesAtStart;
                if (currentValue == value)
                {
                    return;
                }

                this.configuration.CheckForUpdatesAtStart = value;
                this.configurationService.SaveConfiguration();

                this.RaisePropertyChanged(nameof(this.UpdateCheckAtStartup));
            }
        }

        public Version CurrentVersion => Assembly.GetExecutingAssembly().GetName().Version;

        private void CheckForUpdates()
        {
            this.IsChecking = true;
            var checker = new UpdateChecker(this.UpdateChecked);
            checker.CheckForUpdates();
        }

        private void UpdateChecked()
        {
            this.IsChecking = false;
        }
    }
}