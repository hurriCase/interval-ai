using CustomUtils.Runtime.Extensions.Observables;
using R3;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.UI.Components.Button;
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

        private IProgressRepository _progressRepository;

        [Inject]
        internal void Inject(IProgressRepository progressRepository)
        {
            _progressRepository = progressRepository;
        }

        internal void Init()
        {
            _progressRepository.NewWordsDailyTarget.SubscribeToTextUntilDestroy(_dailyWordGoalText);
            _progressRepository.HasDailyTarget.SubscribeToInteractableUntilDestroy(_minusButton);

            _minusButton.OnClickAsObservable()
                .SubscribeUntilDestroy(this, static self => self._progressRepository.ChangeDailyTarget(-1));

            _plusButton.OnClickAsObservable()
                .SubscribeUntilDestroy(this, static self => self._progressRepository.ChangeDailyTarget(+1));
        }
    }
}