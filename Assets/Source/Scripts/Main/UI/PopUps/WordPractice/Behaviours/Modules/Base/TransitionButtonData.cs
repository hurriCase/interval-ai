using System;
using System.Threading;
using R3;
using Source.Scripts.Core.Configs;
using Source.Scripts.UI.Components;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base
{
    [Serializable]
    internal struct TransitionButtonData
    {
        [field: SerializeField] internal ButtonComponent Button { get; private set; }
        [field: SerializeField] internal ModuleType ModuleType { get; private set; }

        internal readonly void Init(CardBehaviour cardBehaviour, CancellationToken cancellationToken)
        {
            Button.OnClickAsObservable()
                .Subscribe((cardBehaviour, ModuleType),
                    static (_, tuple) => tuple.cardBehaviour.SwitchModuleCommand.Execute(tuple.ModuleType))
                .RegisterTo(cancellationToken);
        }
    }
}