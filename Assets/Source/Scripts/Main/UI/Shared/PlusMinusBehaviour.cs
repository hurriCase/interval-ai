using R3;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Progress.Base;
using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.Shared
{
    internal sealed class PlusMinusBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _dailyWordGoalText;

        [SerializeField] private ButtonComponent _minusButton;
        [SerializeField] private ButtonComponent _plusButton;

        [Inject] private IProgressRepository _progressRepository;

        internal void Init()
        {
            var dailyWordsGoal = _progressRepository.DailyWordsGoal;

            dailyWordsGoal.Subscribe(this,
                    static (goal, behaviour) => behaviour._dailyWordGoalText.text = goal.ToString())
                .RegisterTo(destroyCancellationToken);

            _minusButton.OnClickAsObservable()
                .Where(dailyWordsGoal, (_, goal) => goal.Value > 0)
                .Subscribe((behaviour: this, dailyWordsGoal), static (_, tuple) =>
                {
                    tuple.dailyWordsGoal.Value--;
                    tuple.behaviour._minusButton.interactable = tuple.dailyWordsGoal.Value > 0;
                })
                .RegisterTo(destroyCancellationToken);

            _plusButton.OnClickAsObservable()
                .Subscribe(dailyWordsGoal, static (_, goal) => goal.Value++)
                .RegisterTo(destroyCancellationToken);
        }
    }
}