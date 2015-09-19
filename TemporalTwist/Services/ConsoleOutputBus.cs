using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemporalTwist.Services
{

    using Interfaces;
    public class ConsoleOutputBus : IConsoleOutputBus
    {
        public ConsoleOutputBus()
        {
            this.Listeners = new List<Action<string>>();
        }

        public IList<Action<string>> Listeners { get; set; }

        public void ProcessLine(string line)
        {
            foreach(var handler in this.Listeners)
            {
                handler.Invoke(line);
            }
        }
    }
}
