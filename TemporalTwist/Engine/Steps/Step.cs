namespace TemporalTwist.Engine.Steps
{
    using System;

    using Model;
    using Interfaces;
    using Interfaces.Steps;

    public abstract class Step : IStep
    {
        internal Action<string> UpdateConsole { get; set; }

        public abstract void ProcessItem(IJob job, IJobItem item);

        protected void ConsoleUpdate(string message)
        {
            if (this.UpdateConsole != null)
            {
                this.UpdateConsole.Invoke(message);
            }
        }
    }
}