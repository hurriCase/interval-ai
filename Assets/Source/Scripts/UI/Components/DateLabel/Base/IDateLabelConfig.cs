using System.Collections.Generic;

namespace Source.Scripts.UI.Components.DateLabel.Base
{
    internal interface IDateLabelConfig
    {
        List<DisplayRuleData> DisplayRules { get; }
    }
}