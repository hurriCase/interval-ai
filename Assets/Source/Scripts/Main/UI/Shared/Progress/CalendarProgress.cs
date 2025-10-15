using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.UI.Theme;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.UI.Shared.Activity;
using UnityEngine;

namespace Source.Scripts.Main.UI.Shared.Progress
{
    internal sealed class CalendarProgress : ProgressItem
    {
        [SerializeField] private ThemeComponent _progressLabelTheme;
        [SerializeField] private ActivityMapping _activityMapping;

        [SerializeField] private float _alphaForExtraDays;

        internal override void Init(
            EnumArray<LearningState, int> progress,
            string labelText,
            bool isOutsideMonth = true)
        {
            if (isOutsideMonth)
                ApplyOutsideMonthEffect();

            base.Init(progress, labelText, isOutsideMonth is false);

            var dateIdentifierColorType = IsActive ? ActivityState.Active : ActivityState.InActive;
            _activityMapping.SetComponentForState(dateIdentifierColorType, _progressLabelTheme);
        }

        private void ApplyOutsideMonthEffect()
        {
            foreach (var sectionData in progressSections)
                sectionData.RoundedFilledImage.SetAlpha(_alphaForExtraDays);

            progressLabel.SetAlpha(_alphaForExtraDays);
        }
    }
}