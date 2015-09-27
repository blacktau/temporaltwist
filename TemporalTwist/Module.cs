namespace TemporalTwist
{
    using Ninject.Extensions.Factory;
    using Ninject.Modules;

    using TemporalTwist.Engine;
    using TemporalTwist.Engine.Steps;
    using TemporalTwist.Factories;
    using TemporalTwist.Interfaces;
    using TemporalTwist.Interfaces.Steps;
    using TemporalTwist.Services;
    using TemporalTwist.ViewModels;

    public class Module : NinjectModule
    {
        public override void Load()
        {
            this.BindViewModels();
            this.BindSteps();
            this.BindFactories();

            this.Bind<ConfigurationService>().ToSelf().InSingletonScope();
            this.Bind<IConsoleOutputBus>().To<ConsoleOutputBus>().InSingletonScope();
            this.Bind<IJobProcessor>().To<JobProcessor>();
            this.Bind<IStepStateMapper>().To<StepStateMapper>();
        }

        private void BindViewModels()
        {
            this.Bind<MainViewModel>().ToSelf();
            this.Bind<ConsoleViewModel>().ToSelf();
        }

        private void BindFactories()
        {
            this.Bind<JobFactory>().ToSelf();
            this.Bind<IConfigurationViewModelFactory>().ToFactory();
            this.Bind<IJobViewModelFactory>().ToFactory();
            this.Bind<IStepFactory>().ToFactory();
            this.Bind<IJobProcessorFactory>().ToFactory();
        }

        private void BindSteps()
        {
            this.Bind<ICleanupStep>().To<CleanupStep>().InSingletonScope();
            //this.Bind<IDecodingStep>().To<FfmpegDecodingStep>().InSingletonScope();
            this.Bind<IDecodingStep>().To<NaudioDecodingStep>().InSingletonScope();
            //this.Bind<IEncodingStep>().To<FfmpegEncodingStep>().InSingletonScope();
            this.Bind<IEncodingStep>().To<NaudioEncodingStep>().InSingletonScope();
            this.Bind<IFileCopyStep>().To<FileCopyStep>().InSingletonScope();
            this.Bind<IInitialisationStep>().To<InitialisationStep>().InSingletonScope();
            this.Bind<ITagCopyingStep>().To<TagCopyingStep>().InSingletonScope();
            this.Bind<ITempoAdjustmentStep>().To<TempoAdjustmentStep>().InSingletonScope();
        }
    }
}