using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using Cysharp.Text;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Practice;
using Source.Scripts.Main.UI.Shared;
using Source.Scripts.UI.Components;
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

        [Inject] protected ILocalizationKeysDatabase localizationKeysDatabase;
        [Inject] protected IPracticeStateService practiceStateService;
        [Inject] protected ICurrentWordsService currentWordsService;
        [Inject] protected IWindowsController windowsController;

        private PracticeState _currentPracticeState;

        internal void Init(PracticeState practiceState)
        {
            _currentPracticeState = practiceState;

            _plusMinusBehaviour.Init();

            currentWordsService.CurrentWordsByState
                .SubscribeAndRegister(this, (currentWords, self) => self.CheckCompleteness(currentWords));

            LocalizationController.Language.SubscribeAndRegister(this, static self => self.UpdateButtonTexts());

            OnInit();
        }

        protected abstract void OnInit();

        protected void SetState(CompleteType completeType, string additionalInfo = null)
        {
            var localization = localizationKeysDatabase
                .GetCompleteDescriptionLocalization(_currentPracticeState, completeType);

            _completeText.SetTextFormat(localization, additionalInfo);

            _noWordsImage.SetActive(completeType == CompleteType.NoWords);
            _completeImage.SetActive(completeType == CompleteType.Complete);
        }

        private void UpdateButtonTexts()
        {
            var localizationByValue = localizationKeysDatabase.LearningCompleteButtons[_currentPracticeState];

            positiveButton.Text.text = localizationByValue.ButtonPositive.GetLocalization();
            negativeButton.Text.text = localizationByValue.ButtonNegative.GetLocalization();
        }

        private void CheckCompleteness(EnumArray<PracticeState, WordEntry> currentWords)
        {
            var currentWord = currentWords[_currentPracticeState];
            if (currentWord == null)
                SetState(CompleteType.NoWords);
            else if (currentWord.Cooldown > DateTime.Now)
                SetState(CompleteType.Complete);
        }
    }
}