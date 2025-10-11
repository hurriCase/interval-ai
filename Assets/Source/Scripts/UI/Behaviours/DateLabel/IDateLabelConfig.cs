using System.Collections.Generic;

namespace Source.Scripts.UI.Behaviours.DateLabel
{
    internal interface IDateLabelConfig
    {
        List<DisplayRuleData> DisplayRules { get; }
    }
}