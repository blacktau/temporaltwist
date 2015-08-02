// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateChecker.cs" company="None">
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
// </remarks>
// --------------------------------------------------------------------------------------------------------------------
namespace TemporalTwist.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows;
    using System.Xml;

    using TemporalTwist.Views;

    internal class UpdateChecker
    {
        public UpdateChecker(Action checkComplete)
        {
            this.CheckComplete = checkComplete;
        }

        public UpdateChecker()
        {
        }

        public static Version CurrentVersion => Assembly.GetExecutingAssembly().GetName().Version;

        public string DownloadUrl { get; private set; }

        public Version NewVersion { get; private set; }

        private Action CheckComplete { get; }

        public void CheckForUpdates()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += this.DoWork;
            worker.RunWorkerCompleted += this.CompletedUpdateCheck;
            worker.RunWorkerAsync();
        }

        private void CompletedUpdateCheck(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(
                    string.Format("There was an error checking for updates: {0}", e.Error.Message), 
                    "Update Check Error.", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return;
            }

            if (this.NewVersion.CompareTo(CurrentVersion) > 0)
            {
                var model = new UpdateNotificationViewModel(this);
                var window = new UpdateNotificationWindow { DataContext = model };
                model.WindowCloseRequester = window.Close;
                window.ShowDialog();
            }

            this.CheckComplete?.Invoke();
        }

        private void DoWork(object sender, DoWorkEventArgs args)
        {
            var document = new XmlDocument();
            var reader = new XmlTextReader("http://temporaltwist.sf.net/version.xml")
                             {
                                 Normalization = true, 
                                 Namespaces = false
                             };
            document.Load(reader);
            var version = document["version"];
            if (version != null)
            {
                var latest = version["latest"];
                if (latest != null)
                {
                    this.NewVersion = new Version(latest.InnerText);
                }

                var download = version["downloadUrl"];
                if (download != null)
                {
                    this.DownloadUrl = download.InnerText;
                }
            }
        }
    }
}