namespace TemporalTwist
{
    using Ninject.Extensions.Factory;
    using Ninject.Modules;
    using Engine;
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
            BindViewModels();
            
            BindSteps();

            BindFactories();

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
            this.Bind<IFfmpegDecodingStep>().To<FfmpegDecodingStep>().InSingletonScope();
            this.Bind<IFfmpegEncodingStep>().To<FfmpegEncodingStep>().InSingletonScope();
            this.Bind<IFileCopyStep>().To<FileCopyStep>().InSingletonScope();
            this.Bind<IInitialisationStep>().To<InitialisationStep>().InSingletonScope();
            this.Bind<ITagCopyingStep>().To<TagCopyingStep>().InSingletonScope();
            this.Bind<ITempoAdjustmentStep>().To<TempoAdjustmentStep>().InSingletonScope();
        }
    }
}