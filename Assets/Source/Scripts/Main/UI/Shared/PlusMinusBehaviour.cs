using CustomUtils.Runtime.Extensions;
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
            _progressRepository.NewWordsDailyTarget.SubscribeAndRegister(this,
                static (wordsTarget, behaviour) => behaviour._dailyWordGoalText.text = wordsTarget.ToString());

            _progressRepository.HasDailyTarget
                .SubscribeAndRegister(this, static (canReduce, self) => self._minusButton.interactable = canReduce);

            _minusButton.OnClickAsObservable()
                .SubscribeAndRegister(this, static self => self._progressRepository.ChangeDailyTarget(-1));

            _plusButton.OnClickAsObservable()
                .SubscribeAndRegister(this, static self => self._progressRepository.ChangeDailyTarget(+1));
        }
    }
}