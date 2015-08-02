// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExternalExecutionStep.cs" company="None">
//   Copyright (c) 2009, Sean Garrett
//   All rights reserved.
//
//   Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
//   following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and 
//      the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//      the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * The names of the contributors may not be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
//   INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
//   DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
//   SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
//   SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//   WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE 
//   USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// <remarks>
//   Base class for those Steps which wrap the execution of an external command line application.
// </remarks>
// --------------------------------------------------------------------------------------------------------------------

namespace TemporalTwist.Engine.Steps
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;




    internal abstract class ExternalExecutionStep : Step
    {



        private readonly string _executablePath;




        private readonly Action<string> _consoleOutputHandler;










        protected ExternalExecutionStep(string executable, Action<string> consoleOutputHandler)
        {
            _executablePath = GetPathRelativeToAssembly(executable);
            _consoleOutputHandler = consoleOutputHandler;
        }










        protected void Execute(string arguments)
        {
            if (!File.Exists(_executablePath))
            {
                throw new FileNotFoundException(
                    string.Format(CultureInfo.InvariantCulture, "Could not find {0}.", _executablePath));
            }

            var process = InitialiseProcess();
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
            if (assembly != null && assembly.Location != null)
            {
                var assemblyInfo = new FileInfo(assembly.Location);
                if (assemblyInfo.DirectoryName != null)
                {
                    return Path.Combine(assemblyInfo.DirectoryName, path);
                }
            }

            return null;
        }







        private Process InitialiseProcess()
        {
            var process = new Process
                              {
                                  EnableRaisingEvents = false,
                                  StartInfo =
                                      {
                                          FileName = _executablePath,
                                          UseShellExecute = false,
                                          CreateNoWindow = true,
                                          RedirectStandardOutput = true,
                                          RedirectStandardError = true
                                      },
                              };
            process.OutputDataReceived += HandleOutputDataReceived;
            process.ErrorDataReceived += HandleErrorDataReceived;
            return process;
        }






        private void HandleOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (_consoleOutputHandler != null)
            {
                _consoleOutputHandler.Invoke(e.Data);
            }
        }






        private void HandleErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (_consoleOutputHandler != null)
            {
                _consoleOutputHandler.Invoke(e.Data);
            }
        }
    }
}