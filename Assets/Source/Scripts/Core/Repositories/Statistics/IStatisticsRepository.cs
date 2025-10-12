using CustomUtils.Runtime.Storage;
using R3;

namespace Source.Scripts.Core.Repositories.Statistics
{
    internal interface IStatisticsRepository
    {
        PersistentReactiveProperty<bool> IsCompleteOnboarding { get; }
        Observable<Unit> OnNewLogin { get; }
        void MarkNewLogin();
    }
}