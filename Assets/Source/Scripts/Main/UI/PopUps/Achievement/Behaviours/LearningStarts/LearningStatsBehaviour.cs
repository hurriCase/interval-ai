using CustomUtils.Runtime.CustomTypes.Collections;
using R3;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Progress.Base;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress;
using Source.Scripts.Main.Source.Scripts.Main.UI.Shared;
using UnityEngine;
using VContainer;
using ZLinq;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts
{
    internal sealed class LearningStatsBehaviour : MonoBehaviour
    {
        [SerializeField] private ProgressGraphBehaviour _progressGraphBehaviour;
        [SerializeField] private WeekDaysBehaviour _weekDaysBehaviour;
        [SerializeField] private ProgressColorMapping _progressColorMapping;
        [SerializeField] private ProgressItem _totalProgressItem;

        [SerializeField] private EnumArray<LearningState, ProgressDescriptionItem> _progressDescriptionItems =
            new(EnumMode.SkipFirst);

        [Inject] private IProgressRepository _progressRepository;

        internal void Init()
        {
            _progressGraphBehaviour.Init();
            _weekDaysBehaviour.Init();

            _progressRepository.TotalCountByState.Subscribe(this,
                    static (totalCountByState, behaviour) => behaviour.UpdateProgress(totalCountByState))
                .RegisterTo(destroyCancellationToken);
        }

        private void UpdateProgress(EnumArray<LearningState, int> totalCountByState)
        {
            var totalWords = totalCountByState.Values.AsValueEnumerable().Sum().ToString();
            _totalProgressItem.Init(totalCountByState, totalWords, _progressColorMapping);

            foreach (var (state, progressItem) in _progressDescriptionItems.AsTuples())
                progressItem.Init(state, totalCountByState[state], _progressColorMapping);
        }
    }
}