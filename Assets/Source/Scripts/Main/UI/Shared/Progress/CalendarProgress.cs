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
        [SerializeField] [field: Range(0f, 1f)] private float _alphaForExtraDays;

        private bool _isOutsideMonth;

        internal void Init(EnumArray<LearningState, int> progress, string labelText, bool isOutsideMonth)
        {
            _isOutsideMonth = isOutsideMonth;

            base.Init(progress, labelText);
        }

        protected override void OnInit(bool isActive)
        {
            isActive = isActive && _isOutsideMonth is false;

            if (_isOutsideMonth)
                ApplyOutsideMonthEffect();

            var dateIdentifierColorType = isActive ? ActivityState.Active : ActivityState.InActive;
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