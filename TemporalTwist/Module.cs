namespace TemporalTwist
{
    using Ninject.Extensions.Factory;
    using Ninject.Modules;

    using TemporalTwist.Factories;
    using TemporalTwist.Interfaces;
    using TemporalTwist.Services;
    using TemporalTwist.ViewModels;

    public class Module : NinjectModule
    {
        public override void Load()
        {
            this.Bind<MainViewModel>().ToSelf();
            this.Bind<ConfigurationService>().ToSelf().InSingletonScope();
            this.Bind<ConsoleViewModel>().ToSelf();
            this.Bind<JobFactory>().ToSelf();

            this.Bind<IConfigurationViewModelFactory>().ToFactory();
            this.Bind<IJobViewModelFactory>().ToFactory();
        }
    }
}