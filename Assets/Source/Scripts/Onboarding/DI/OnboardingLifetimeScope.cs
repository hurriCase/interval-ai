using Source.Scripts.Onboarding.Source.Scripts.Onboarding.Data;
using Source.Scripts.Onboarding.Source.Scripts.Onboarding.Data.Base;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Onboarding.Source.Scripts.Onboarding.DI
{
    internal sealed class OnboardingLifetimeScope : LifetimeScope
    {
        [SerializeField] private WindowsController _windowsController;
        [SerializeField] private LocalizationDatabase _localizationDatabase;
        [SerializeField] private WordGoalDatabase _wordGoalDatabase;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_windowsController).As<IWindowsController>();
            builder.RegisterComponent(_localizationDatabase).As<ILocalizationDatabase>();
            builder.RegisterInstance(_wordGoalDatabase).As<IWordGoalDatabase>();

            builder.RegisterEntryPoint<OnboardingEntryPoint>();
        }
    }
}