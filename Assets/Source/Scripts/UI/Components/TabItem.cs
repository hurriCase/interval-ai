using System;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Toggles;
using UnityEngine;

namespace Source.Scripts.UI.Components
{
    [Serializable]
    internal sealed class TabItem
    {
        [field: SerializeField] internal CanvasGroup CanvasGroup { get; private set; }
        [field: SerializeField] internal StateToggle SwitchToggle { get; private set; }
    }
}