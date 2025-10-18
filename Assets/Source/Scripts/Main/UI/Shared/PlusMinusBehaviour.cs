using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using R3;
using Source.Scripts.Core.Repositories.Progress.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Shared
{
    internal sealed class PlusMinusBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _dailyWordGoalText;

        [SerializeField] private ThemeButton _minusButton;
        [SerializeField] private ThemeButton _plusButton;

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