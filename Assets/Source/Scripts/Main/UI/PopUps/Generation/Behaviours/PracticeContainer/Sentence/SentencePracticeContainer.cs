using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using CustomUtils.Runtime.UI.Theme;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Exercises.Exercise;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Generation.Behaviours.PracticeContainer.Sentence
{
    internal sealed class SentencePracticeContainer : PracticeContainerBase
    {
        [SerializeField] private TextMeshProUGUI _sentenceText;
        [SerializeField] private SentencePracticeMapping _sentencePracticeMapping;
        [SerializeField] private ThemeComponent _practiceTheme;
        [SerializeField] private ThemeButton _practiceButton;
        [SerializeField] private TextMeshProUGUI _remarkText;
        [SerializeField] private TMP_InputField _answerInputField;

        [SerializeField] private EnumArray<SentencePracticeState, Sprite> _practiceImages;
        [SerializeField] private Image _practiceIcon;

        private ILocalizationDatabase _localizationDatabase;

        private ExerciseEntry _currentExerciseEntry;

        [Inject]
        internal void Inject(ILocalizationDatabase localizationDatabase)
        {
            _localizationDatabase = localizationDatabase;
        }

        internal override void Init()
        {
            _practiceButton.OnClickAsObservable().SubscribeUntilDestroy(this, static self => self.CheckSentence());
        }

        internal override void UpdateView(ExerciseEntry exerciseEntry)
        {
            _currentExerciseEntry = exerciseEntry;

            _sentenceText.text = exerciseEntry.Content.Learning;

            SetPracticeState(SentencePracticeState.Check, false);
        }

        private void CheckSentence()
        {
            var answer = _currentExerciseEntry.Content.Native;
            var answerState = string.Equals(answer, _answerInputField.text, StringComparison.OrdinalIgnoreCase)
                ? SentencePracticeState.Correct
                : SentencePracticeState.Error;

            SetPracticeState(answerState, true);
        }

        private void SetPracticeState(SentencePracticeState practiceState, bool isAnswer)
        {
            _sentencePracticeMapping.SetComponentForState(practiceState, _practiceTheme);

            _practiceIcon.sprite = _practiceImages[practiceState];

            var practiceData = _localizationDatabase.SentencePractices[practiceState];
            _practiceButton.Text.text = practiceData.PracticeButtonKey.GetLocalization();

            _remarkText.text = practiceData.RemarkKey.GetLocalization();
            _remarkText.SetActive(isAnswer);

            _practiceButton.interactable = isAnswer is false;
        }
    }
}