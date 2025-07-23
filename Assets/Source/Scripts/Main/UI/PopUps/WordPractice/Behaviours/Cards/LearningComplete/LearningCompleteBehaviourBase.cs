using CustomUtils.Runtime.Localization;
using R3;
using Source.Scripts.Core.Localization;
using Source.Scripts.Main.Source.Scripts.Main.UI.Shared;
using Source.Scripts.UI.Selectables;
using Source.Scripts.UI.Windows.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.LearningComplete
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

        [Inject] protected IWindowsController windowsController;

        private PracticeState _practiceState;

        internal void Init(PracticeState state)
        {
            _practiceState = state;

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
                .Subscribe(windowsController,
                    static (_, controller) => controller.OpenScreenByType(ScreenType.LearningWords))
                .RegisterTo(destroyCancellationToken);

            OnInit();
        }

        internal void SetState(CompleteState state, string newWordCount = null)
        {
            var localizationKey = LocalizationKeysDatabase.Instance.CompleteLocalizationData[_practiceState][state];

            _completeText.text = string.Format(LocalizationController.Localize(localizationKey), newWordCount);

            _noWordsImage.SetActive(state == CompleteState.NoWords);
            _completeImage.SetActive(state == CompleteState.Complete);
        }

        protected abstract void OnInit();
    }
}