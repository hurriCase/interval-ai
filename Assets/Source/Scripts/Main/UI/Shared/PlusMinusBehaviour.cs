using R3;
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
            var wordsTarget = _progressRepository.NewWordsDailyTarget;

            wordsTarget.Subscribe(this,
                    static (goal, behaviour) => behaviour._dailyWordGoalText.text = goal.ToString())
                .RegisterTo(destroyCancellationToken);

            _minusButton.OnClickAsObservable()
                .Where(wordsTarget, (_, goal) => goal.Value > 0)
                .Subscribe((behaviour: this, wordsTarget), static (_, tuple) =>
                {
                    tuple.wordsTarget.Value--;
                    tuple.behaviour._minusButton.interactable = tuple.wordsTarget.Value > 0;
                })
                .RegisterTo(destroyCancellationToken);

            _plusButton.OnClickAsObservable()
                .Subscribe(wordsTarget, static (_, wordsTarget) => wordsTarget.Value++)
                .RegisterTo(destroyCancellationToken);
        }
    }
}