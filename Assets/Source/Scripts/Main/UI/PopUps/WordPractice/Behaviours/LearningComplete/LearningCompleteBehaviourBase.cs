using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.Localization;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using Cysharp.Text;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Base.CurrentWord;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete.CompleteState;
using Source.Scripts.Main.UI.Shared;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete
{
    internal abstract class LearningCompleteBehaviourBase : MonoBehaviour
    {
        [SerializeField] protected GameObject buttonsContainer;
        [SerializeField] protected ThemeButton positiveButton;
        [SerializeField] protected ThemeButton negativeButton;

        [SerializeField] private TextMeshProUGUI _completeText;
        [SerializeField] private GameObject _noWordsImage;
        [SerializeField] private GameObject _completeImage;

        [SerializeField] private PlusMinusBehaviour _plusMinusBehaviour;

        [Inject] protected IPracticeStateService practiceStateService;
        [Inject] protected IWindowsController windowsController;

        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;
        [Inject] private ICompleteServiceFactory _completeStateFactory;
        [Inject] private ICurrentWordFactory _currentWordFactory;

        protected ICurrentWordService currentWordService;
        private PracticeState _currentPracticeState;

        internal void Init(PracticeState practiceState)
        {
            _currentPracticeState = practiceState;

            currentWordService = _currentWordFactory.GetOrCreate(practiceState);

            _plusMinusBehaviour.Init();

            LocalizationController.Language.SubscribeUntilDestroy(this, static self => self.UpdateButtonTexts());

            var completeStateService = _completeStateFactory.GetOrCreate(practiceState);
            completeStateService.CompleteStates
                .SubscribeUntilDestroy(this, static (completeType, self) => self.CheckCompleteness(completeType));

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