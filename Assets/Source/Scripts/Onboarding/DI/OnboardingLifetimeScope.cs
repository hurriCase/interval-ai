using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Onboarding.Source.Scripts.Onboarding.DI
{
    internal sealed class OnboardingLifetimeScope : LifetimeScope
    {
        [SerializeField] private WindowsController _windowsController;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_windowsController).As<IWindowsController>();

            builder.RegisterEntryPoint<OnboardingEntryPoint>();
        }
    }
}