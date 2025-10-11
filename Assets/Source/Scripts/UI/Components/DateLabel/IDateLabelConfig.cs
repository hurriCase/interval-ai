using System.Collections.Generic;

namespace Source.Scripts.UI.Components.DateLabel
{
    internal interface IDateLabelConfig
    {
        List<DisplayRuleData> DisplayRules { get; }
    }
}