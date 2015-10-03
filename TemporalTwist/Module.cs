namespace TemporalTwist
{
    using Ninject;
    using Ninject.Activation;
    using Ninject.Extensions.Factory;
    using Ninject.Modules;

    using TemporalTwist.Engine;
    using TemporalTwist.Engine.Steps;
    using TemporalTwist.Factories;
    using TemporalTwist.Interfaces;
    using TemporalTwist.Interfaces.Factories;
    using TemporalTwist.Interfaces.Services;
    using TemporalTwist.Interfaces.Steps;
    using TemporalTwist.Interfaces.Views;
    using TemporalTwist.Services;
    using TemporalTwist.ViewModels;
    using TemporalTwist.Views;

    public class Module : NinjectModule
    {
        public override void Load()
        {
            this.BindViews();
            this.BindViewModels();
            this.BindSteps();
            this.BindFactories();
            this.BindServices();

            this.Bind<IConsoleOutputBus>().To<ConsoleOutputBus>().InSingletonScope();
            this.Bind<IJobProcessor>().To<JobProcessor>();
            this.Bind<IStepStateMapper>().To<StepStateMapper>();
        }

        private Configuration.Configuration GetConfiguration(IContext arg)
        {
            var configurationService = arg.Kernel.Get<IConfigurationService>();
            return configurationService.GetConfiguration();
        }

        private void BindViews()
        {
            this.Bind<IConsoleWindow>().To<ConsoleWindow>();
            this.Bind<IConfigurationEditorWindow>().To<ConfigurationEditorWindow>();
        }

        private void BindViewModels()
        {
            this.Bind<MainViewModel>().ToSelf();
            this.Bind<ConsoleViewModel>().ToSelf();
            this.Bind<ConfigurationViewModel>().ToSelf();
        }

        private void BindServices()
        {
            this.Bind<IConfigurationService>().To<ConfigurationService>().InSingletonScope();
            this.Bind<IShutdownService>().To<ShutdownService>().InSingletonScope();
            this.Bind<IWindowService>().To<WindowService>().InSingletonScope();
        }

        private void BindFactories()
        {
            this.Bind<IJobFactory>().To<JobFactory>();
            this.Bind<IConfigurationViewModelFactory>().ToFactory();
            this.Bind<IJobViewModelFactory>().ToFactory();
            this.Bind<IStepFactory>().ToFactory();
            this.Bind<IJobProcessorFactory>().ToFactory();
            this.Bind<IViewFactory>().ToFactory();
        }

        private void BindSteps()
        {
            this.Bind<ICleanupStep>().To<CleanupStep>().InSingletonScope();
            this.Bind<IDecodingStep>().To<NaudioDecodingStep>().InSingletonScope();
            this.Bind<IEncodingStep>().To<NaudioEncodingStep>().InSingletonScope();
            this.Bind<IFileCopyStep>().To<FileCopyStep>().InSingletonScope();
            this.Bind<IInitialisationStep>().To<InitialisationStep>().InSingletonScope();
            this.Bind<ITagCopyingStep>().To<TagCopyingStep>().InSingletonScope();
            this.Bind<ITempoAdjustmentStep>().To<TempoAdjustmentStep>().InSingletonScope();
        }
    }
}