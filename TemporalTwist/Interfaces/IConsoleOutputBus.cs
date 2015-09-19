using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemporalTwist.Interfaces
{
    public interface IConsoleOutputBus
    {
        IList<Action<string>> Listeners { get; set; }

        void ProcessLine(string line);
    }
}
