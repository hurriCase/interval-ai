using R3;
using Source.Scripts.Core.Localization;
using Source.Scripts.Main.Source.Scripts.Main.Data.Base;
using Source.Scripts.Main.Source.Scripts.Main.UI.Shared;
using Source.Scripts.UI.Components;
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

        [SerializeField] private TextMeshProUGUI _completeText;
        [SerializeField] private GameObject _noWordsImage;
        [SerializeField] private GameObject _completeImage;

        [SerializeField] private GameObject _addNewWordsContainer;
        [SerializeField] private ButtonComponent _exitButton;
        [SerializeField] private ButtonComponent _learnButton;

        [SerializeField] private PlusMinusBehaviour _plusMinusBehaviour;

        [Inject] protected IWindowsController windowsController;

        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;

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
                .Subscribe(static _ => WordPracticePopUp.CurrentState.OnNext(PracticeState.NewWords))
                .RegisterTo(destroyCancellationToken);

            _exitButton.OnClickAsObservable()
                .Subscribe(windowsController,
                    static (_, controller) => controller.OpenScreenByType(ScreenType.LearningWords))
                .RegisterTo(destroyCancellationToken);

            OnInit();
        }

        internal void SetState(CompleteState state, string newWordCount = null)
        {
            var localization = _localizationKeysDatabase.GetCompletesLocalization(_practiceState, state);
            _completeText.text = string.Format(localization, newWordCount);

            _noWordsImage.SetActive(state == CompleteState.NoWords);
            _completeImage.SetActive(state == CompleteState.Complete);
        }

        protected abstract void OnInit();
    }
}