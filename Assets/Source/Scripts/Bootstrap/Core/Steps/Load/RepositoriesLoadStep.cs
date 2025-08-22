using Source.Scripts.Core.Repositories.Base;
using UnityEngine;

namespace Source.Scripts.Bootstrap.Core.Steps.Load
{
    [CreateAssetMenu(
        fileName = nameof(RepositoriesLoadStep),
        menuName = InitializationStepsPath + nameof(RepositoriesLoadStep)
    )]
    internal sealed class RepositoriesLoadStep : LoadStepBase<IRepository> { }
}