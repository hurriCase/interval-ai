using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Core.Others
{
    [Serializable]
    internal class ComponentWithSpacing<TComponent> where TComponent : Component
    {
        [field: SerializeField] internal TComponent Component { get; private set; }

        [SerializeField] private AspectRatioFitter _spacing;

        internal void Toggle(bool isVisible)
        {
            Component.gameObject.SetActive(isVisible);
            _spacing.gameObject.SetActive(isVisible);
        }
    }
}