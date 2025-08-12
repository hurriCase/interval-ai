using Source.Scripts.Core.Localization.Base;
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
        [SerializeField] private LocalizationDatabase _localizationDatabase;
        [SerializeField] private OnboardingConfig _onboardingConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_windowsController).As<IWindowsController>();
            builder.RegisterComponent(_localizationDatabase).As<ILocalizationDatabase>();
            builder.RegisterInstance(_onboardingConfig).As<IOnboardingConfig>();

            builder.RegisterEntryPoint<OnboardingEntryPoint>();
        }
    }
}