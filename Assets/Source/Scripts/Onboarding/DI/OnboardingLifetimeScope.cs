using Source.Scripts.Core.Repositories.Words;
using Source.Scripts.Core.Repositories.Words.Advance;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Practice;
using Source.Scripts.Onboarding.Data.Config;
using Source.Scripts.Onboarding.Data.CurrentWords;
using Source.Scripts.Onboarding.UI.Base;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Onboarding.DI
{
    internal sealed class OnboardingLifetimeScope : LifetimeScope
    {
        [SerializeField] private OnboardingWindowsController _windowsController;
        [SerializeField] private OnboardingConfig _onboardingConfig;
        [SerializeField] private DefaultOnboardingDatabase _defaultOnboardingDatabase;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_windowsController).AsImplementedInterfaces();
            builder.RegisterInstance(_onboardingConfig).AsImplementedInterfaces();

            builder.Register<PracticeStateService>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<WordsTimerService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WordAdvanceService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<OnboardingCurrentWordsService>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.RegisterInstance(_defaultOnboardingDatabase)
                .As<DefaultOnboardingDatabase>()
                .AsImplementedInterfaces();

            builder.RegisterEntryPoint<OnboardingEntryPoint>();
        }
    }
}