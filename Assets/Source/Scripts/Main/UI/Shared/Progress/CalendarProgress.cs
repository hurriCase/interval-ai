using CustomUtils.Runtime.Extensions;
using UnityEngine;

namespace Source.Scripts.Main.UI.Shared.Progress
{
    internal sealed class CalendarProgress : ProgressItem
    {
        [SerializeField] private ProgressColorMapping _progressColorMapping;
        [SerializeField] private GameObject _fireIcon;
        [SerializeField] [field: Range(0f, 1f)] private float _alphaForExtraDays;

        private bool _isOutsideMonth;

        protected override void OnInit(bool isActive)
        {
            isActive = isActive && !_isOutsideMonth;

            _fireIcon.SetActive(isActive);

            if (_isOutsideMonth)
                ApplyOutsideMonthEffect();
        }

        internal void UpdateView(bool isOutsideMonth)
        {
            _isOutsideMonth = isOutsideMonth;
        }

        private void ApplyOutsideMonthEffect()
        {
            foreach (var sectionData in progressSections)
                sectionData.RoundedFilledImage.SetAlpha(_alphaForExtraDays);

            dateIdentifierText.SetAlpha(_alphaForExtraDays);
        }
    }
}