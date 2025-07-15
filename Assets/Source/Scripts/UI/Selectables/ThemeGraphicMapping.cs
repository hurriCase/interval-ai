using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Selectables
{
    [Serializable]
    internal struct ThemeGraphicMapping
    {
        [SerializeField] private Graphic _targetGraphic;
        [SerializeField] private SelectableColorMapping _colorMapping;

        internal void ApplyColor(SelectableStateType state)
        {
            if (!_colorMapping || !_targetGraphic)
                return;

            _targetGraphic.color = _colorMapping.GetColorForState(state);
        }
    }
}