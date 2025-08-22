using Source.Scripts.Core.Repositories.Base.DefaultConfig;
using UnityEngine;

namespace Source.Scripts.Bootstrap.Core.Steps.Load
{
    [CreateAssetMenu(
        fileName = nameof(DefaultDatabaseLoadStep),
        menuName = InitializationStepsPath + nameof(DefaultDatabaseLoadStep)
    )]
    internal sealed class DefaultDatabaseLoadStep : LoadStepBase<IDefaultDataDatabase> { }
}