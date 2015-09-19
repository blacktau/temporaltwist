using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemporalTwist.Engine;

namespace TemporalTwist.Interfaces
{
    public interface IJobProcessorFactory
    {
        IJobProcessor CreateJobProcessor(Action<IJob> processingFinishedCallback);
    }
}
