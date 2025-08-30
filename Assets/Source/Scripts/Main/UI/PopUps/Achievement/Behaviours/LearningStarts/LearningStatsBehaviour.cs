using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress;
using Source.Scripts.Main.UI.Shared;
using UnityEngine;
using VContainer;
using ZLinq;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts
{
    internal sealed class LearningStatsBehaviour : MonoBehaviour
    {
        [SerializeField] private ProgressGraphBehaviour _progressGraphBehaviour;
        [SerializeField] private WeekDaysBehaviour _weekDaysBehaviour;
        [SerializeField] private ProgressColorMapping _progressColorMapping;
        [SerializeField] private ProgressItem _totalProgressItem;

        [SerializeField] private EnumArray<LearningState, ProgressDescriptionItem> _progressDescriptionItems =
            new(EnumMode.SkipFirst);

        private IProgressRepository _progressRepository;

        [Inject]
        internal void Inject(IProgressRepository progressRepository)
        {
            _progressRepository = progressRepository;
        }

        internal void Init()
        {
            _progressGraphBehaviour.Init();
            _weekDaysBehaviour.Init();

            _progressRepository.TotalCountByState
                .SubscribeAndRegister(this, static (totalCountByState, self) => self.UpdateProgress(totalCountByState));
        }

        private void UpdateProgress(EnumArray<LearningState, int> totalCountByState)
        {
            var totalWords = totalCountByState.Entries.AsValueEnumerable().Sum(entry => entry.Value).ToString();
            _totalProgressItem.Init(totalCountByState, totalWords, _progressColorMapping);

            foreach (var (state, progressItem) in _progressDescriptionItems.AsTuples())
                progressItem.Init(state, totalCountByState[state], _progressColorMapping);
        }
    }
}