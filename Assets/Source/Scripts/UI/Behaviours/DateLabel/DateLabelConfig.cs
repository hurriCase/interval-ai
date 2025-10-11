using System.Collections.Generic;
using Source.Scripts.UI.Behaviours.DateLabel.Base;
using UnityEngine;

namespace Source.Scripts.UI.Behaviours.DateLabel
{
    [CreateAssetMenu(menuName = nameof(DateLabelConfig), fileName = nameof(DateLabelConfig))]
    internal sealed class DateLabelConfig : ScriptableObject, IDateLabelConfig
    {
        [field: SerializeField] public List<DisplayRuleData> DisplayRules { get; private set; }
    }
}