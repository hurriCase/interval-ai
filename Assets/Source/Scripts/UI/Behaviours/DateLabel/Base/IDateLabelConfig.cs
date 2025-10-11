using System.Collections.Generic;

namespace Source.Scripts.UI.Behaviours.DateLabel.Base
{
    internal interface IDateLabelConfig
    {
        List<DisplayRuleData> DisplayRules { get; }
    }
}