using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Practice;
using Source.Scripts.Onboarding.Data;
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

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_windowsController).AsImplementedInterfaces();
            builder.RegisterInstance(_onboardingConfig).AsImplementedInterfaces();

            builder.Register<PracticeStateService>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.RegisterEntryPoint<OnboardingEntryPoint>();
        }
    }
}