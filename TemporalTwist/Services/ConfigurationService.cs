// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationService.cs" company="None">
//   Copyright (c) 2011, Sean Garrett
//   All rights reserved.
//   Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
//   following conditions are met:
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and the 
//      following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the 
//      following disclaimer in the documentation and/or other materials provided with the distribution.
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
//   INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
//   DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
//   SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
//   SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//   WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE 
//   USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE
// </copyright>
// <remarks>
// </remarks>
// --------------------------------------------------------------------------------------------------------------------
namespace TemporalTwist.Services
{
    using System.IO;
    using System.Reflection;

    using Newtonsoft.Json;

    using TemporalTwist.Configuration;

    public class ConfigurationService
    {
        private Configuration configuration;

        public Configuration GetConfiguration()
        {
            if (this.configuration == null)
            {
                var configurationFile = GetConfigurationFile();
                this.configuration = JsonConvert.DeserializeObject<Configuration>(configurationFile);
            }

            return this.configuration;
        }

        public void SaveConfiguration()
        {
            var json = JsonConvert.SerializeObject(this.configuration, Formatting.Indented);
            File.WriteAllText(GetConfigurationFilePath(), json);
        }

        private static string GetConfigurationFile()
        {
            return File.ReadAllText(GetConfigurationFilePath());
        }

        private static string GetConfigurationFilePath()
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (directory != null)
            {
                return Path.Combine(directory, "configuration.json");
            }

            throw new FileNotFoundException(Assembly.GetExecutingAssembly().Location);
        }
    }
}