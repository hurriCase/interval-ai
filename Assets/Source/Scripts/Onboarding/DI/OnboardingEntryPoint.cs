using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Source.Scripts.Onboarding.Source.Scripts.Onboarding.DI
{
    internal sealed class OnboardingEntryPoint : IAsyncStartable
    {
        public async UniTask StartAsync(CancellationToken cancellationToken) { }
    }
}