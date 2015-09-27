namespace TemporalTwist.Factories
{
    using System;
    using System.Collections.Generic;

    using TemporalTwist.Interfaces;
    using TemporalTwist.Interfaces.Steps;
    using TemporalTwist.Model;

    public class StepStateMapper : IStepStateMapper
    {
        private readonly IStepFactory stepFactory;

        private readonly IDictionary<JobItemState, Func<IStep>> stepMap;

        public StepStateMapper(IStepFactory stepFactory)
        {
            this.stepFactory = stepFactory;
            this.stepMap = new Dictionary<JobItemState, Func<IStep>>();

            this.InitialiseStepMap();
        }

        public int Count
        {
            get
            {
                return this.stepMap.Count;
            }
        }

        public IStep GetStepForState(JobItemState itemState)
        {
            if (!this.stepMap.ContainsKey(itemState))
            {
                return null;
            }

            return this.stepMap[itemState].Invoke();
        }

        private void InitialiseStepMap()
        {
            this.stepMap.Add(JobItemState.None, this.stepFactory.CreateInitialisationStep);
            this.stepMap.Add(JobItemState.Initialised, this.stepFactory.CreateFfmpegDecodingStep);
            this.stepMap.Add(JobItemState.Decoded, this.stepFactory.CreateTempoAdjustmentStep);
            this.stepMap.Add(JobItemState.TempoAdjusted, this.stepFactory.CreateFfmpegEncodingStep);
            this.stepMap.Add(JobItemState.Encoded, this.stepFactory.CreateTagCopyingStep);
            this.stepMap.Add(JobItemState.Tagged, this.stepFactory.CreateFileCopyStep);
            this.stepMap.Add(JobItemState.Copied, this.stepFactory.CreateCleanupStep);
        }
    }
}