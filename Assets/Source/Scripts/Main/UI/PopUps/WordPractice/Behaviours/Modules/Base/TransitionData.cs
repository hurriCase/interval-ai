using System;
using Source.Scripts.Core.Configs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base
{
    [Serializable]
    internal struct TransitionData<TUIBehaviour>
        where TUIBehaviour : UIBehaviour
    {
        [field: SerializeField] internal TUIBehaviour TransitionObject { get; private set; }
        [field: SerializeField] internal ModuleType ModuleType { get; private set; }
    }
}