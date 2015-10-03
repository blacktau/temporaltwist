namespace TemporalTwist.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows;
    using System.Xml;

    using TemporalTwist.Views;

    public class UpdateChecker
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
/* TODO FIX THIS!!!
                var model = new UpdateNotificationViewModel(this);
                var window = new UpdateNotificationWindow { DataContext = model };
                window.ShowDialog();
*/
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