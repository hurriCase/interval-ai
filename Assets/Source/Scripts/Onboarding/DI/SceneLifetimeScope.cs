using Source.Scripts.Core.Repositories.Words.Advance;
using Source.Scripts.Core.Repositories.Words.ModuleState;
using Source.Scripts.Onboarding.Data.Config;
using Source.Scripts.Onboarding.Data.CurrentWords;
using Source.Scripts.Onboarding.UI.Base;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using PracticeStateService = Source.Scripts.Core.Repositories.Words.PracticeStateService;
using WordsTimerService = Source.Scripts.Core.Repositories.Words.WordsTimerService;

namespace Source.Scripts.Onboarding.DI
{
    internal sealed class SceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private WindowsController _windowsController;
        [SerializeField] private OnboardingConfig _onboardingConfig;
        [SerializeField] private DefaultWordsDatabase _defaultWordsDatabase;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_windowsController).AsImplementedInterfaces();
            builder.RegisterInstance(_onboardingConfig).AsImplementedInterfaces();

            builder.Register<ModuleServiceFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WordAdvanceFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CurrentWordFactory>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<CurrentWordService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PracticeStateService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WordsTimerService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance(_defaultWordsDatabase)
                .As<DefaultWordsDatabase>()
                .AsImplementedInterfaces();

            builder.RegisterEntryPoint<SceneEntryPoint>();
        }
    }
}