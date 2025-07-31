using R3;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Shared
{
    internal sealed class PlusMinusBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _dailyWordGoalText;

        [SerializeField] private ButtonComponent _minusButton;
        [SerializeField] private ButtonComponent _plusButton;

        [Inject] private IProgressRepository _progressRepository;

        internal void Init()
        {
            _progressRepository.NewWordsDailyTarget.Subscribe(this,
                    static (wordsTarget, behaviour)
                        => behaviour._dailyWordGoalText.text = wordsTarget.ToString())
                .RegisterTo(destroyCancellationToken);

            _minusButton.OnClickAsObservable()
                .Subscribe(this, static (_, behaviour) =>
                {
                    var wordsTarget = behaviour._progressRepository.NewWordsDailyTarget;
                    wordsTarget.Value--;
                    behaviour._minusButton.interactable = wordsTarget.Value > 0;
                })
                .RegisterTo(destroyCancellationToken);

            _plusButton.OnClickAsObservable()
                .Subscribe(this, static (_, behaviour)
                    => behaviour._progressRepository.NewWordsDailyTarget.Value++)
                .RegisterTo(destroyCancellationToken);
        }
    }
}