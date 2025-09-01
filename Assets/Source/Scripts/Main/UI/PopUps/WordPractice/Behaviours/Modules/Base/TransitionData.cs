using System;
using System.Threading;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Configs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base
{
    [Serializable]
    internal struct TransitionData
    {
        [field: SerializeField] internal UIBehaviour TransitionObject { get; private set; }
        [field: SerializeField] internal ModuleType ModuleType { get; private set; }

        internal readonly void Init(CardBehaviour cardBehaviour, CancellationToken cancellationToken)
        {
            TransitionObject.OnPointerClickAsObservable()
                .Subscribe((cardBehaviour, ModuleType),
                    static (_, tuple) => tuple.cardBehaviour.SwitchModuleCommand.Execute(tuple.ModuleType))
                .RegisterTo(cancellationToken);
        }
    }
}