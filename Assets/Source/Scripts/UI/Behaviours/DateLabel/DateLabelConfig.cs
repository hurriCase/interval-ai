using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.UI.Behaviours.DateLabel
{
    [CreateAssetMenu(menuName = nameof(DateLabelConfig), fileName = nameof(DateLabelConfig))]
    internal sealed class DateLabelConfig : ScriptableObject, IDateLabelConfig
    {
        [field: SerializeField] public List<DisplayRuleData> DisplayRules { get; private set; }
    }
}