// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateNotificationViewModel.cs" company="None">
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
//   ViewModel for the Update Notification View
// </remarks>
// --------------------------------------------------------------------------------------------------------------------
namespace TemporalTwist.ViewModels
{
    using System;
    using System.Diagnostics;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    using TemporalTwist.Core;

    internal class UpdateNotificationViewModel : BaseViewModel
    {
        private readonly UpdateChecker updateChecker;

        public UpdateNotificationViewModel(UpdateChecker updateChecker)
        {
            this.updateChecker = updateChecker;
            this.DownloadCommand = new RelayCommand(this.LaunchDownload);
        }

        public ICommand DownloadCommand { get; private set; }

        public Version CurrentVersion => UpdateChecker.CurrentVersion;

        public Version NewVersion => this.updateChecker.NewVersion;

        public string DownloadUrl => this.updateChecker.DownloadUrl;

        private void LaunchDownload()
        {
            Process.Start(this.DownloadUrl);
        }
    }
}