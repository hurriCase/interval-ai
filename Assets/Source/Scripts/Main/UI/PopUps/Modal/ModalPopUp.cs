using System;
using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes.Modal;
using Source.Scripts.UI.Components.Button;
using Source.Scripts.UI.Windows.Base;
using TMPro;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Modal
{
    internal sealed class ModalPopUp : PopUpBase
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private TextMeshProUGUI _positiveText;
        [SerializeField] private TextMeshProUGUI _negativeText;
        [SerializeField] private ButtonComponent _positiveButton;
        [SerializeField] private ButtonComponent _negativeButton;

        private IDisposable _disposable;

        internal void SetParameters<TSource>(
            ModalLocalizationData localizationData,
            TSource source,
            Action<TSource> positiveAction = null,
            Action<TSource> negativeAction = null)
        {
            _titleText.text = localizationData.TitleKey.GetLocalization();
            _messageText.text = localizationData.MessageKey.GetLocalization();
            _positiveText.text = localizationData.PositiveKey.GetLocalization();
            _negativeText.text = localizationData.NegativeKey.GetLocalization();

            _disposable?.Dispose();

            var positiveDisposable = _positiveButton.OnClickAsObservable()
                .Subscribe((positiveAction, source), static (_, tuple) => tuple.positiveAction?.Invoke(tuple.source));

            var negativeDisposable = _negativeButton.OnClickAsObservable()
                .Subscribe((negativeAction, source), static (_, tuple) => tuple.negativeAction?.Invoke(tuple.source));

            _disposable = Disposable.Combine(positiveDisposable, negativeDisposable);
        }

        internal override void Init()
        {
            _negativeButton.OnClickAsObservable()
                .SubscribeAndRegister(this, static self => self.HideAsync().Forget());

            _positiveButton.OnClickAsObservable()
                .SubscribeAndRegister(this, static self => self.HideAsync().Forget());
        }
    }
}