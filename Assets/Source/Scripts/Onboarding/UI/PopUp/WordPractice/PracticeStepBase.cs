using System;
using System.Threading;
using CustomUtils.Runtime.Extensions;
using R3;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Onboarding.UI.PopUp.WordPractice
{
    [Serializable]
    internal abstract class PracticeStepBase
    {
        [field: SerializeField] internal bool IsTransition { get; private set; }

        [SerializeField] private string _localizationKey;
        [SerializeField] private Vector2 _textAnchorMin;
        [SerializeField] private Vector2 _textAnchorMax;

        internal Observable<Unit> ButtonClickObservable => buttonClickSubject.AsObservable();
        protected Subject<Unit> buttonClickSubject = new();

        internal abstract void Init(Transform tintParent, CancellationToken cancellationToken);
        internal abstract void ActiveStep();
        internal abstract void HideStep();

        internal void UpdateText(TextMeshProUGUI messageText)
        {
            messageText.rectTransform.anchorMin = _textAnchorMin;
            messageText.rectTransform.anchorMax = _textAnchorMax;
            messageText.text = _localizationKey.GetLocalization();
        }
    }
}