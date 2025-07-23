using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Onboarding.Source.Scripts.Onboarding.DI
{
    internal sealed class OnboardingLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<OnboardingEntryPoint>();
        }
    }
}