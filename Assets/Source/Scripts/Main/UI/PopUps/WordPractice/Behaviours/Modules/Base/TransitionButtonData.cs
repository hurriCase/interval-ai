using System;
using System.Threading;
using R3;
using Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.Base;
using Source.Scripts.UI.Selectables;
using UnityEngine;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base
{
    [Serializable]
    internal struct TransitionButtonData
    {
        [field: SerializeField] internal ButtonComponent Button { get; private set; }
        [field: SerializeField] internal ModuleType ModuleType { get; private set; }

        internal void Init(CancellationToken token)
        {
            Button.OnClickAsObservable()
                .Subscribe(ModuleType, static (_, type) => WordPracticePopUp.ModuleChangeRequested.OnNext(type))
                .RegisterTo(token);
        }
    }
}