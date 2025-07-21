using CustomUtils.Runtime.Localization;
using R3;
using Source.Scripts.UI.Localization;
using Source.Scripts.UI.Selectables;
using Source.Scripts.UI.Windows.Base;
using Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.Base;
using Source.Scripts.UI.Windows.Shared;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.LearningComplete
{
    internal abstract class LearningCompleteBehaviourBase : MonoBehaviour
    {
        [SerializeField] private GameObject _buttonsContainer;
        [SerializeField] private ButtonComponent _learnNewWordsButton;
        [SerializeField] private ButtonComponent _returnLateButton;

        [SerializeField] private TextMeshProUGUI _completeText;
        [SerializeField] private GameObject _noWordsImage;
        [SerializeField] private GameObject _completeImage;

        [SerializeField] private GameObject _addNewWordsContainer;
        [SerializeField] private ButtonComponent _learnButton;

        [SerializeField] private PlusMinusBehaviour _plusMinusBehaviour;

        [SerializeField] protected ButtonComponent exitButton;

        protected PracticeState practiceState;

        internal void Init(PracticeState state)
        {
            practiceState = state;

            _plusMinusBehaviour.Init();

            _learnNewWordsButton.OnClickAsObservable()
                .Subscribe(this, static (_, behaviour) =>
                {
                    behaviour._buttonsContainer.SetActive(false);
                    behaviour._addNewWordsContainer.SetActive(true);
                })
                .RegisterTo(destroyCancellationToken);

            _learnButton.OnClickAsObservable()
                .Subscribe(static _ => WordPracticePopUp.StateChangeRequested.OnNext(PracticeState.NewWords))
                .RegisterTo(destroyCancellationToken);

            _returnLateButton.OnClickAsObservable()
                .Subscribe(static _ => WindowsController.Instance.OpenScreenByType(ScreenType.LearningWords))
                .RegisterTo(destroyCancellationToken);

            OnInit();
        }

        internal void SetState(CompleteState state, string newWordCount = null)
        {
            var localizationKey = LocalizationKeysDatabase.Instance.CompleteLocalizationData[practiceState][state];

            _completeText.text = string.Format(LocalizationController.Localize(localizationKey), newWordCount);

            _noWordsImage.SetActive(state == CompleteState.NoWords);
            _completeImage.SetActive(state == CompleteState.Complete);
        }

        protected abstract void OnInit();
    }
}