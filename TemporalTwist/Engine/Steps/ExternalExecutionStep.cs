namespace TemporalTwist.Engine.Steps
{
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;

    using TemporalTwist.Interfaces;

    public abstract class ExternalExecutionStep : Step
    {
        private readonly IConsoleOutputBus consoleOutputHandler;

        private readonly string executablePath;

        protected ExternalExecutionStep(string executable, IConsoleOutputBus consoleOutputHandler)
        {
            this.executablePath = GetPathRelativeToAssembly(executable);
            this.consoleOutputHandler = consoleOutputHandler;
        }

        protected void Execute(string arguments)
        {
            if (!File.Exists(this.executablePath))
            {
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Could not find {0}.", this.executablePath));
            }

            var process = this.InitialiseProcess();
            process.StartInfo.Arguments = arguments;
            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            process.WaitForExit();
            process.CancelErrorRead();
            process.CancelOutputRead();
            process.Close();
        }

        private static string GetPathRelativeToAssembly(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyInfo = new FileInfo(assembly.Location);
            if (assemblyInfo.DirectoryName != null)
            {
                return Path.Combine(assemblyInfo.DirectoryName, path);
            }

            return null;
        }

        private Process InitialiseProcess()
        {
            var process = new Process { EnableRaisingEvents = false, StartInfo = { FileName = this.executablePath, UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true } };
            process.OutputDataReceived += this.HandleOutputDataReceived;
            process.ErrorDataReceived += this.HandleErrorDataReceived;
            return process;
        }

        private void HandleOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.consoleOutputHandler?.ProcessLine(e.Data);
        }

        private void HandleErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.consoleOutputHandler?.ProcessLine(e.Data);
        }
    }
}