using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using Cysharp.Text;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Practice;
using Source.Scripts.Main.UI.Shared;
using Source.Scripts.UI.Components.Button;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete
{
    internal abstract class LearningCompleteBehaviourBase : MonoBehaviour
    {
        [SerializeField] protected GameObject buttonsContainer;
        [SerializeField] protected ButtonTextComponent positiveButton;
        [SerializeField] protected ButtonTextComponent negativeButton;

        [SerializeField] private TextMeshProUGUI _completeText;
        [SerializeField] private GameObject _noWordsImage;
        [SerializeField] private GameObject _completeImage;

        [SerializeField] private PlusMinusBehaviour _plusMinusBehaviour;

        [Inject] protected IPracticeStateService practiceStateService;
        [Inject] protected ICurrentWordsService currentWordsService;
        [Inject] protected IWindowsController windowsController;

        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;
        [Inject] private ICompleteStateService _completeStateService;

        private PracticeState _currentPracticeState;

        internal void Init(PracticeState practiceState)
        {
            _currentPracticeState = practiceState;

            _plusMinusBehaviour.Init();

            LocalizationController.Language.SubscribeAndRegister(this, static self => self.UpdateButtonTexts());

            _completeStateService.CompleteStates
                .Select(_currentPracticeState, (completeTypes, state) => completeTypes[state])
                .SubscribeAndRegister(this, static (completeType, self) => self.CheckCompleteness(completeType));

            OnInit();
        }

        protected abstract void OnInit();

        protected void SetState(CompleteType completeType, string additionalInfo = null)
        {
            var localization = _localizationKeysDatabase
                .GetCompleteDescriptionLocalization(_currentPracticeState, completeType);

            _completeText.SetTextFormat(localization, additionalInfo);

            _noWordsImage.SetActive(completeType == CompleteType.NoWords);
            _completeImage.SetActive(completeType == CompleteType.Complete);
        }

        private void CheckCompleteness(CompleteType completeType)
        {
            if (completeType == CompleteType.NoWords)
            {
                SetState(completeType);
                return;
            }

            OnCheckCompleteness(completeType);
        }

        protected abstract void OnCheckCompleteness(CompleteType completeType);

        private void UpdateButtonTexts()
        {
            var localizationByValue = _localizationKeysDatabase.LearningCompleteButtons[_currentPracticeState];

            positiveButton.Text.text = localizationByValue.ButtonPositive.GetLocalization();
            negativeButton.Text.text = localizationByValue.ButtonNegative.GetLocalization();
        }
    }
}